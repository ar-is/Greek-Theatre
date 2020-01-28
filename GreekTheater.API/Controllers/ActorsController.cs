using AutoMapper;
using GreekTheater.API.Core.Dtos.Actor;
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
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetActors")]
        [HttpHead]
        public ActionResult<IEnumerable<ActorDto>> GetActors(string name)
        {
            var actor = _mapper.Map<IEnumerable<ActorDto>>(_unitOfWork.Actors.GetActors(name));

            return Ok(actor);
        }

        [HttpGet("{actorId}", Name = "GetActor")]
        [HttpHead("{actorId}")]
        public ActionResult<ActorDto> GetActor(Guid actorId)
        {
            var actor = _unitOfWork.Actors.GetActor(actorId);

            if (actor == null)
                return NotFound();

            return Ok(_mapper.Map<ActorDto>(actor));
        }

        [HttpPost(Name = "CreateActor")]
        [RequestHeaderMatchesMediaType("Content-Type",
            "application/json",
            "application/vnd.marvin.actorforcreation+json")]
        public ActionResult<ActorDto> CreateDirector(ActorForCreationDto actor)
        {
            var actorEntity = _mapper.Map<Actor>(actor);

            if (_unitOfWork.Actors.ActorExists(actorEntity))
                return Conflict(new { message = $"This Actor already exists in the database!" });

            SaveActorInDB(actorEntity);

            var actorToReturn = _mapper.Map<ActorDto>(actorEntity);

            return CreatedAtRoute("GetActor",
                                  new { actorId = actorToReturn.Id },
                                  actorToReturn);
        }

        [HttpPost(Name = "CreateActorWithDateOfDeath")]
        [RequestHeaderMatchesMediaType("Content-Type",
            "application/vnd.marvin.actorforcreationwithdateofdeath+json")]
        public IActionResult CreateActorWithDateOfDeath(ActorForCreationWithDateOfDeathDto actor)
        {
            var actorEntity = _mapper.Map<Actor>(actor);

            if (_unitOfWork.Actors.ActorExists(actorEntity))
                return Conflict(new { message = $"This Actor already exists in the database!" });

            SaveActorInDB(actorEntity);

            var actorToReturn = _mapper.Map<ActorDto>(actorEntity);

            return CreatedAtRoute("GetActor",
                                  new { actorId = actorToReturn.Id },
                                  actorToReturn);
        }

        private void SaveActorInDB(Actor actor)
        {
            if (actor.Id == 0)
                _unitOfWork.Actors.AddActor(actor);
            else
                _unitOfWork.Actors.UpdateActor(actor);

            _unitOfWork.Complete();
        }

        private IActionResult UpsertActor(Guid actorId, ActorForUpdateDto actorToBeUpdated)
        {
            var actorToAdd = _mapper.Map<Actor>(actorToBeUpdated);

            if (_unitOfWork.Actors.ActorExists(actorToAdd))
                return Conflict(new { message = $"This Actor already exists in the database!" });

            actorToAdd.Guid = actorId;
            SaveActorInDB(actorToAdd);

            var actorToReturn = _mapper.Map<ActorDto>(actorToAdd);

            return CreatedAtRoute("GetActor",
                                  new { actorId = actorToReturn.Id },
                                  actorToReturn);
        }

        [HttpPut("{actorId}")]
        public IActionResult UpdateActor(Guid actorId, ActorForUpdateDto actorToBeUpdated)
        {
            var actorFromDb = _unitOfWork.Actors.GetActor(actorId);

            if (actorFromDb == null)
                return UpsertActor(actorId, actorToBeUpdated);

            _mapper.Map(actorToBeUpdated,actorFromDb);
            SaveActorInDB(actorFromDb);

            return NoContent();
        }

        [HttpPatch("{actorId}")]
        public IActionResult PartiallyUpdateActor(Guid actorId,
            JsonPatchDocument<ActorForUpdateDto> patchDocument)
        {
            var actorFromDb = _unitOfWork.Actors.GetActor(actorId);

            if (actorFromDb == null)
            {
                var actorDto = new ActorForUpdateDto();
                patchDocument.ApplyTo(actorDto, ModelState);

                if (!TryValidateModel(actorDto))
                    return ValidationProblem(ModelState);

                return UpsertActor(actorId, actorDto);
            }

            var actorToPatch = _mapper.Map<ActorForUpdateDto>(actorFromDb);
            patchDocument.ApplyTo(actorToPatch, ModelState);

            if (!TryValidateModel(actorToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(actorToPatch, actorFromDb);

            SaveActorInDB(actorFromDb);

            return NoContent();
        }

        [HttpDelete("{actorId}")]
        public ActionResult DeleteActor(Guid actorId)
        {
            var actorFromDb = _unitOfWork.Actors.GetActor(actorId);

            if (actorFromDb == null)
                return NotFound();

            _unitOfWork.Actors.DeleteActor(actorFromDb);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetActorsOptions()
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
