using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Repositories;
using GreekTheater.API.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly GreekTheaterAPIContext _context;

        public ActorRepository(GreekTheaterAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private IEnumerable<Actor> GetActors()
        {
            return _context.Actors
                .Include(a => a.Actings)
                    .ThenInclude(a => a.Performance)
                .ToList();
        }

        public IEnumerable<Actor> GetActors(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetActors();

            name = name.Trim();

            return _context.Actors
                .Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name))
                .Include(a => a.Actings)
                    .ThenInclude(a => a.Performance)
                .ToList();
        }

        public IEnumerable<Actor> GetActors(IEnumerable<Guid> actorIds)
        {
            return _context.Actors.Where(a => actorIds.Contains(a.Guid))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public Actor GetActor(Guid actorId)
        {
            if (actorId == Guid.Empty)
                throw new ArgumentNullException(nameof(actorId));

            return _context.Actors
                .Include(a => a.Actings)
                    .ThenInclude(a => a.Performance)
                .FirstOrDefault(a => a.Guid == actorId);
        }

        public bool ActorExists(Actor actor)
        {
            return _context.Actors.Any(a => a.FirstName == actor.FirstName &&
                                            a.LastName == actor.LastName &&
                                            a.DateOfBirth == actor.DateOfBirth);
        }

        public bool ActorExists(Guid actorId)
        {
            if (actorId == Guid.Empty)
                throw new ArgumentNullException(nameof(actorId));

            return _context.Actors.Any(d => d.Guid == actorId);
        }

        public bool ActorsExist(IEnumerable<Actor> actors)
        {
            foreach (var actor in actors)
            {
                return _context.Actors.Any(a => a.FirstName == actor.FirstName &&
                                                   a.LastName == actor.LastName &&
                                                   a.DateOfBirth == actor.DateOfBirth);
            }

            return false;
        }

        public void AddActor(Actor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (actor.Guid == Guid.Empty)
                actor.Guid = Guid.NewGuid();

            _context.Actors.Add(actor);
        }

        public void UpdateActor(Actor actor)
        {

        }

        public void DeleteActor(Actor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            _context.Actors.Remove(actor);
        }
    }
}
