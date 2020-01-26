using AutoMapper;
using GreekTheater.API.Core.Dtos.Director;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Services;
using GreekTheater.API.Helpers.ActionConstraints;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Controllers
{
    [ApiController]
    [Route("api/directors")]
    public class DirectorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DirectorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetDirectors")]
        [HttpHead]
        public ActionResult<IEnumerable<DirectorDto>> GetDirectors(string name)
        {
            return Ok(_mapper.Map<IEnumerable<DirectorDto>>(_unitOfWork.Directors.GetDirectors(name)));
        }

        [HttpGet("{directorId}", Name = "GetDirector")]
        [HttpHead]
        public ActionResult<DirectorDto> GetDirector(Guid directorId)
        {
            var director = _unitOfWork.Directors.GetDirector(directorId);

            if (director == null)
                return NotFound();

            return Ok(_mapper.Map<DirectorDto>(director));
        }

        [HttpPost(Name = "CreateDirector")]
        [RequestHeaderMatchesMediaType("Content-Type",
            "application/json",
            "application/vnd.marvin.directorforcreation+json")]
        public ActionResult<DirectorDto> CreateDirector(DirectorForCreationDto director)
        {
            var directorEntity = _mapper.Map<Director>(director);

            if (_unitOfWork.Directors.DirectorExists(directorEntity))
                return Conflict(new { message = $"This Director already exists in the database!" });

            SaveDirectorInDB(directorEntity);

            var directorToReturn = _mapper.Map<DirectorDto>(directorEntity);

            return CreatedAtRoute("GetDirector",
                                  new { directorId = directorToReturn.Id },
                                  directorToReturn);
        }

        [HttpPost(Name = "CreateDirectorWithDateOfDeath")]
        [RequestHeaderMatchesMediaType("Content-Type",
            "application/vnd.marvin.directorforcreationwithdateofdeath+json")]
        public IActionResult CreateDirectorWithDateOfDeath(DirectorForCreationWithDateOfDeathDto director)
        {
            var directorEntity = _mapper.Map<Director>(director);

            if (_unitOfWork.Directors.DirectorExists(directorEntity))
                return Conflict(new { message = $"This Director already exists in the database!" });

            SaveDirectorInDB(directorEntity);

            var directorToReturn = _mapper.Map<DirectorDto>(directorEntity);

            return CreatedAtRoute("GetDirector",
                                  new { directorId = directorToReturn.Id },
                                  directorToReturn);
        }

        private void SaveDirectorInDB(Director director)
        {
            if (director.Id == 0)
                _unitOfWork.Directors.AddDirector(director);
            else
                _unitOfWork.Directors.UpdateDirector(director);

            _unitOfWork.Complete();
        }

        private IActionResult UpsertDirector(Guid directorId, DirectorForUpdateDto directorToBeUpdated)
        {
            var directorToAdd = _mapper.Map<Director>(directorToBeUpdated);

            if (_unitOfWork.Directors.DirectorExists(directorToAdd))
                return Conflict(new { message = $"This Director already exists in the database!" });

            directorToAdd.Guid = directorId;
            SaveDirectorInDB(directorToAdd);

            var directorToReturn = _mapper.Map<DirectorDto>(directorToAdd);

            return CreatedAtRoute("GetDirector",
                                  new { directorId = directorToReturn.Id },
                                  directorToReturn);
        }

        [HttpPut("{directorId}")]
        public IActionResult UpdateDirector(Guid directorId, DirectorForUpdateDto directorToBeUpdated)
        {
            var directorFromDb = _unitOfWork.Directors.GetDirector(directorId);

            if (directorFromDb == null)
                return UpsertDirector(directorId, directorToBeUpdated);

            _mapper.Map(directorToBeUpdated, directorFromDb);
            SaveDirectorInDB(directorFromDb);

            return NoContent();
        }

        [HttpPatch("{directorId}")]
        public IActionResult PartiallyUpdateDirector(Guid directorId,
            JsonPatchDocument<DirectorForUpdateDto> patchDocument)
        {
            var directorFromDb = _unitOfWork.Directors.GetDirector(directorId);

            if (directorFromDb == null)
            {
                var directorDto = new DirectorForUpdateDto();
                patchDocument.ApplyTo(directorDto, ModelState);

                if (!TryValidateModel(directorDto))
                    return ValidationProblem(ModelState);

                return UpsertDirector(directorId, directorDto);
            }

            var directorToPatch = _mapper.Map<DirectorForUpdateDto>(directorFromDb);
            patchDocument.ApplyTo(directorToPatch, ModelState);

            if (!TryValidateModel(directorToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(directorToPatch, directorFromDb);

            SaveDirectorInDB(directorFromDb);

            return NoContent();
        }

        [HttpDelete("{directorId}")]
        public ActionResult DeleteDirector(Guid directorId)
        {
            var directorFromDb = _unitOfWork.Directors.GetDirector(directorId);

            if (directorFromDb == null)
                return NotFound();

            _unitOfWork.Directors.DeleteDirector(directorFromDb);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetDirectorsOptions()
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
