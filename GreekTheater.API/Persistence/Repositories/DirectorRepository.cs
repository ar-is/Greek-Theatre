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
    public class DirectorRepository : IDirectorRepository
    {
        private readonly GreekTheaterAPIContext _context;

        public DirectorRepository(GreekTheaterAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private IEnumerable<Director> GetDirectors()
        {
            return _context.Directors
                .Include(d => d.Performances)
                .ToList();
        }

        public IEnumerable<Director> GetDirectors(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetDirectors();

            name = name.Trim();

            return _context.Directors
                .Where(d => d.FirstName.Contains(name) || d.LastName.Contains(name))
                .Include(d => d.Performances)
                .ToList();
        }

        public IEnumerable<Director> GetDirectors(IEnumerable<Guid> directorIds)
        {
            return _context.Directors.Where(d => directorIds.Contains(d.Guid))
                .OrderBy(d => d.FirstName)
                .OrderBy(d => d.LastName)
                .ToList();
        }

        public Director GetDirector(Guid directorId)
        {
            if (directorId == Guid.Empty)
                throw new ArgumentNullException(nameof(directorId));

            return _context.Directors
                .Include(d => d.Performances)
                    .ThenInclude(m => m.PerformanceGenres)
                .FirstOrDefault(d => d.Guid == directorId);
        }

        public bool DirectorExists(Director director)
        {
            return _context.Directors.Any(d => d.FirstName == director.FirstName &&
                                               d.LastName == director.LastName &&
                                               d.DateOfBirth == director.DateOfBirth);
        }

        public bool DirectorExists(Guid directorId)
        {
            if (directorId == Guid.Empty)
                throw new ArgumentNullException(nameof(directorId));

            return _context.Directors.Any(d => d.Guid == directorId);
        }

        public bool DirectorsExist(IEnumerable<Director> directors)
        {
            foreach (var director in directors)
            {
                return _context.Directors.Any(d => d.FirstName == director.FirstName &&
                                                   d.LastName == director.LastName &&
                                                   d.DateOfBirth == director.DateOfBirth);
            }

            return false;
        }

        public void AddDirector(Director director)
        {
            if (director == null)
                throw new ArgumentNullException(nameof(director));

            if (director.Guid == Guid.Empty)
                director.Guid = Guid.NewGuid();

            _context.Directors.Add(director);
        }

        public void UpdateDirector(Director director)
        {

        }

        public void DeleteDirector(Director director)
        {
            if (director == null)
                throw new ArgumentNullException(nameof(director));

            _context.Directors.Remove(director);
        }
    }
}
