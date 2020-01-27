using GreekTheater.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Repositories
{
    public interface IActorRepository
    {
        IEnumerable<Actor> GetActors(string name);
        IEnumerable<Actor> GetActors(IEnumerable<Guid> actorIds);
        Actor GetActor(Guid actorId);
        bool ActorExists(Actor actor);
        bool ActorExists(Guid actorId);
        bool ActorsExist(IEnumerable<Actor> actors);
        void AddActor(Actor actor);
        void UpdateActor(Actor actor);
        void DeleteActor(Actor actor);
    }
}
