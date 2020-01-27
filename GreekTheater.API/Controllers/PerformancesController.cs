using AutoMapper;
using GreekTheater.API.Core.Dtos.Performance;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Services;
using GreekTheater.API.Core.Services.Data_Shaping;
using GreekTheater.API.Core.Services.Pagination;
using GreekTheater.API.Core.Services.Sorting;
using GreekTheater.API.Helpers.Extension_Methods;
using GreekTheater.API.Helpers.ResourceFilters;
using GreekTheater.API.Helpers.VendorMediaTypes;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Controllers
{
    [ApiController]
    [Route("api/directors/{directorId}/performances")]
    public class PerformancesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;
        private readonly IPaginationService<Performance> _paginationService;
        private readonly IDataShapingService<Performance> _dataShapingService;
        private readonly IDependentPaginationService<Performance> _dependentLinksService;

        public PerformancesController(IUnitOfWork unitOfWork, IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService,
            IPaginationService<Performance> paginationService,
            IDependentPaginationService<Performance> dependentLinksService,
            IDataShapingService<Performance> dataShapingService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ??
                throw new ArgumentNullException(nameof(propertyCheckerService));
            _paginationService = paginationService ??
                throw new ArgumentNullException(nameof(paginationService));
            _dependentLinksService = dependentLinksService
                ?? throw new ArgumentNullException(nameof(dependentLinksService));
            _dataShapingService = dataShapingService
                ?? throw new ArgumentNullException(nameof(dataShapingService));
        }

        [HttpGet("/api/performances", Name = "GetPerformances")]
        [HttpHead("/api/performances")]
        public IActionResult GetPerformances([FromQuery] PerformancesResourceParameters parameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<PerformanceDto, Performance>(parameters.OrderBy) ||
                !_propertyCheckerService.TypeHasProperties<PerformanceDto>(parameters.Fields))
                return BadRequest();

            var performances = _unitOfWork.Performances.GetPerformances(parameters);

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(
                    _paginationService.CreatePaginationMetadata(performances, parameters)));

            return Ok(_dataShapingService.GetShapedCollectionWithLinks(performances, parameters));
        }

        [HttpGet(Name = "GetPerformancesForDirector")]
        [HttpHead]
        public ActionResult<IEnumerable<PerformanceDto>> GetPerformancesForDirector(Guid directorId)
        {
            if (!_unitOfWork.Directors.DirectorExists(directorId))
                return NotFound();

            return Ok(_mapper
                .Map<IEnumerable<PerformanceDto>>(_unitOfWork.Performances.GetPerformances(directorId)));
        }

        [Produces(MediaTypes.Json,
            MediaTypes.HateoasPlusJson,
            MediaTypes.FullPerformanceJson,
            MediaTypes.FullPerformanceHateoasPlusJson,
            MediaTypes.FriendlyPerformanceJson,
            MediaTypes.FriendlyPerformanceHateoasPlusJson)]
        [HttpGet("/api/performances/{performanceId}", Name = "GetPerformance")]
        [HttpHead("/api/performances/{performanceId}")]
        public IActionResult GetPerformance(Guid performanceId, string fields,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType.Contains("full"))
            {
                if (!_propertyCheckerService.TypeHasProperties<PerformanceFullDto>(fields))
                    return BadRequest();
            }
            else
            {
                if (!_propertyCheckerService.TypeHasProperties<PerformanceDto>(fields))
                    return BadRequest();
            }

            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
                return BadRequest();

            var performance = _unitOfWork.Performances.GetPerformance(performanceId);

            if (performance == null)
                return NotFound();

            return Ok(_dataShapingService.GetShapedEntity(parsedMediaType, performance, fields));
        }

        [HttpGet("{performanceId}", Name = "GetPerformanceForDirector")]
        [HttpHead("{performanceId}")]
        public ActionResult<PerformanceDto> GetPerformanceForDirector(Guid directorId, Guid performanceId)
        {
            if (!_unitOfWork.Directors.DirectorExists(directorId))
                return NotFound();

            var performanceFromDb = _unitOfWork.Performances.GetPerformance(performanceId);

            if (performanceFromDb == null)
                return NotFound();

            return Ok(_mapper.Map<PerformanceDto>(performanceFromDb));
        }

        [HttpPost(Name = "CreatePerformance")]
        public ActionResult<PerformanceDto> CreatePerformance(Guid directorId,
            [FromBody]PerformanceForCreationDto performance)
        {
            if (!_unitOfWork.Directors.DirectorExists(directorId))
                return NotFound();

            var performanceEntity = _mapper.Map<Performance>(performance);

            if (_unitOfWork.Performances.PerformanceExists(directorId, performanceEntity))
                return Conflict(new { message = $"This Performance already exists in the database!" });

            SavePerformanceInDb(directorId, performanceEntity);

            var performanceToReturn = _mapper.Map<PerformanceDto>(
                _unitOfWork.Performances.GetPerformance(performanceEntity.Guid));

            var linkedResourceToReturn = performanceToReturn.ShapeData(null)
                                         as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", _dependentLinksService.
                    CreateLinksForDependentEntity(directorId, performanceToReturn.Id, null));

            return CreatedAtRoute("GetPerformanceForDirector",
                                  new { directorId, 
                                        performanceId = linkedResourceToReturn["Id"] },
                                        linkedResourceToReturn);
        }

        private void SavePerformanceInDb(Guid directorId, Performance performance)
        {
            if (performance.Id == 0)
                _unitOfWork.Performances.AddPerformance(directorId, performance);
            else
                _unitOfWork.Performances.UpdatePerformance(performance);

            _unitOfWork.Complete();
        }

        private IActionResult UpsertPerformance(Guid directorId, Guid performanceId, 
            PerformanceForUpdateDto performanceToBeUpdated)
        {
            var performanceToAdd = _mapper.Map<Performance>(performanceToBeUpdated);

            if (_unitOfWork.Performances.PerformanceExists(directorId, performanceToAdd))
                return Conflict(new { message = $"This Performance already exists in the database!" });

            performanceToAdd.Guid = performanceId;
            SavePerformanceInDb(directorId, performanceToAdd);

            var performanceToReturn = _mapper.Map<PerformanceDto>(
                _unitOfWork.Performances.GetPerformance(performanceToAdd.Guid));

            return CreatedAtRoute("GetPerformanceForDirector",
                                  new { directorId,
                                        performanceId = performanceToReturn.Id },
                                        performanceToReturn);
        }

        [HttpPut("{performanceId}", Name = "UpdatePerformance")]
        public IActionResult UpdatePerformance([FromRoute]Guid directorId, 
            Guid performanceId, PerformanceForUpdateDto performanceToBeUpdated)
        {
            if (!_unitOfWork.Directors.DirectorExists(directorId))
                return NotFound();

            var performanceFromDb = _unitOfWork.Performances.GetPerformance(performanceId);

            if (performanceFromDb == null)
                return UpsertPerformance(directorId, performanceId, performanceToBeUpdated);

            _mapper.Map(performanceToBeUpdated, performanceFromDb);
            SavePerformanceInDb(directorId, performanceFromDb);

            return NoContent();
        }

        [HttpPatch("{performanceId}", Name = "PartiallyUpdatePerformance")]
        public IActionResult PartiallyUpdatePerformance(Guid directorId, Guid performanceId,
            JsonPatchDocument<PerformanceForUpdateDto> patchDocument)
        {
            if (!_unitOfWork.Directors.DirectorExists(directorId))
                return NotFound();

            var performanceFromDb = _unitOfWork.Performances.GetPerformance(performanceId);

            if (performanceFromDb == null)
            {
                var performanceDto = new PerformanceForUpdateDto();
                patchDocument.ApplyTo(performanceDto, ModelState);

                if (!TryValidateModel(performanceDto))
                    return ValidationProblem(ModelState);

                return UpsertPerformance(directorId, performanceId, performanceDto);
            }

            var performanceToPatch = _mapper.Map<PerformanceForUpdateDto>(performanceFromDb);
            patchDocument.ApplyTo(performanceToPatch, ModelState);

            if (!TryValidateModel(performanceToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(performanceToPatch, performanceFromDb);

            SavePerformanceInDb(directorId, performanceFromDb);

            return NoContent();
        }

        [HttpDelete("/api/performances/{performanceId}", Name = "DeletePerformance")]
        public ActionResult DeletePerformance(Guid performanceId)
        {
            var performanceFromDb = _unitOfWork.Performances.GetPerformance(performanceId);

            if (performanceFromDb == null)
                return NotFound();

            _unitOfWork.Performances.DeletePerformance(performanceFromDb);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{performanceId}")]
        public ActionResult DeletePerformance(Guid directorId, Guid performanceId)
        {
            if (!_unitOfWork.Directors.DirectorExists(directorId))
                return NotFound();

            var performanceFromDb = _unitOfWork.Performances.GetPerformance(directorId, performanceId);

            if (performanceFromDb == null)
                return NotFound();

            _unitOfWork.Performances.DeletePerformance(performanceFromDb);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetPerformancesOptions()
        {
            Response.Headers.Add("Allow", "DELETE,GET,OPTIONS,POST,PUT");

            return Ok();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
