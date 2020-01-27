using GreekTheater.API.Core.Dtos.Performance;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Repositories;
using GreekTheater.API.Core.Services.Sorting;
using GreekTheater.API.Helpers.Extension_Methods;
using GreekTheater.API.Helpers.Paging;
using GreekTheater.API.Helpers.ResourceFilters;
using GreekTheater.API.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Repositories
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly GreekTheaterAPIContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public PerformanceRepository(GreekTheaterAPIContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService
               ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public Performance GetPerformance(Guid performanceId)
        {
            if (performanceId == Guid.Empty)
                throw new ArgumentNullException(nameof(performanceId));

            return _context.Performances
                .Include(p => p.Director)
                .Include(p => p.Actings)
                    .ThenInclude(a => a.Actor)
                .Include(p => p.PerformanceGenres)
                    .ThenInclude(pg => pg.Genre)
                .SingleOrDefault(p => p.Guid == performanceId);
        }

        public Performance GetPerformance(Guid directorId, Guid performanceId)
        {
            if (directorId == Guid.Empty)
                throw new ArgumentNullException(nameof(directorId));

            var director = _context.Directors.FirstOrDefault(d => d.Guid == directorId);

            if (performanceId == Guid.Empty)
                throw new ArgumentNullException(nameof(performanceId));

            return _context.Performances
                .Include(p => p.Director)
                .Include(p => p.Actings)
                    .ThenInclude(a => a.Actor)
                .Include(p => p.PerformanceGenres)
                    .ThenInclude(pg => pg.Genre)
                .FirstOrDefault(p => p.DirectorId == director.Id && p.Guid == performanceId);
        }

        public IEnumerable<Performance> GetPerformances(Guid directorId)
        {
            if (directorId == Guid.Empty)
                throw new ArgumentNullException(nameof(directorId));

            var director = _context.Directors.FirstOrDefault(d => d.Guid == directorId);

            return _context.Performances
                .Include(p => p.Director)
                .Include(p => p.Actings)
                    .ThenInclude(a => a.Actor)
                .Include(p => p.PerformanceGenres)
                    .ThenInclude(pg => pg.Genre)
                .Where(p => p.DirectorId == director.Id)
                .ToList();
        }

        public IEnumerable<Performance> GetPerformances(IEnumerable<Guid> performanceIds)
        {
            return _context.Performances
                .Include(p => p.Director)
                .Include(p => p.Actings)
                    .ThenInclude(a => a.Actor)
                .Include(p => p.PerformanceGenres)
                    .ThenInclude(pg => pg.Genre)
                .Where(p => performanceIds.Contains(p.Guid))
                .ToList();
        }

        public PagedList<Performance> GetPerformances(PerformancesResourceParameters parameters)
        {
            IQueryable<Performance> performances = Enumerable.Empty<Performance>().AsQueryable();

            if (parameters.Filter != null)
            {
                foreach (var condition in GetPerformancesDictionary(parameters.Filter).Keys)
                {
                    if (condition)
                        performances = GetPerformancesDictionary(parameters.Filter)[condition];
                }
            }
            else
                performances = GetPerformances();

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                var moviePropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<PerformanceDto, Performance>();

                performances = performances.ApplySort(parameters.OrderBy,
                    moviePropertyMappingDictionary);
            }

            return PagedList<Performance>.Create(performances,
                parameters.PageNumber,
                parameters.PageSize);
        }

        private Dictionary<bool, IQueryable<Performance>> GetPerformancesDictionary(PerformancesFilter filter)
        {
            return new Dictionary<bool, IQueryable<Performance>>
            {
                [filter.WithTitle()] = GetPerformancesFilteredByTitle(filter),
                [filter.WithTitleAndDirector()] = GetPerformancesFilteredByTitleAndDirector(filter),
                [filter.WithTitleAndYear()] = GetPerformancesFilteredByTitleAndYear(filter),
                [filter.WithTitleDirectorAndYear()] = GetPerformancesFilteredByTitleDirectorAndYear(filter),
                [filter.WithYear()] = GetPerformancesFilteredByYear(filter),
                [filter.WithYearAndDirector()] = GetPerformancesFilteredByYearAndDirector(filter),
                [filter.WithDirector()] = GetPerformancesFilteredByDirector(filter)
            };
        }

        private IQueryable<Performance> GetPerformances()
        {
            return _context.Performances
                .Include(p => p.Director)
                .Include(p => p.Actings)
                    .ThenInclude(a => a.Actor)
                .Include(p => p.PerformanceGenres)
                    .ThenInclude(pg => pg.Genre);
        }

        private IQueryable<Performance> GetPerformancesFilteredByDirector(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.Director.FirstName.Contains(filter.Director) ||
                                  p.Director.LastName.Contains(filter.Director));
        }

        private IQueryable<Performance> GetPerformancesFilteredByYearAndDirector(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.Director.FirstName.Contains(filter.Director) ||
                                  p.Director.LastName.Contains(filter.Director))
                      .Where(p => p.PremiereDate.HasValue ? p.PremiereDate.Value.Year == filter.PremiereYear : true);

        }

        private IQueryable<Performance> GetPerformancesFilteredByYear(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.PremiereDate.HasValue ? p.PremiereDate.Value.Year == filter.PremiereYear : true);
        }

        private IQueryable<Performance> GetPerformancesFilteredByTitle(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.Title.Contains(filter.Title));
        }

        private IQueryable<Performance> GetPerformancesFilteredByTitleDirectorAndYear(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.Director.FirstName.Contains(filter.Director) ||
                                  p.Director.LastName.Contains(filter.Director))
                      .Where(p => p.Title.Contains(filter.Title) &&
                                  p.PremiereDate.HasValue ? p.PremiereDate.Value.Year == filter.PremiereYear : true);
        }

        private IQueryable<Performance> GetPerformancesFilteredByTitleAndYear(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.Title.Contains(filter.Title))
                      .Where(p => p.PremiereDate.HasValue ? p.PremiereDate.Value.Year == filter.PremiereYear : true);
        }

        private IQueryable<Performance> GetPerformancesFilteredByTitleAndDirector(PerformancesFilter filter)
        {
            return GetPerformances()
                      .Where(p => p.Director.FirstName.Contains(filter.Director) ||
                                  p.Director.LastName.Contains(filter.Director))
                      .Where(p => p.Title.Contains(filter.Title));
        }

        public bool PerformanceExists(Guid performanceId)
        {
            if (performanceId == Guid.Empty)
                throw new ArgumentNullException(nameof(performanceId));

            return _context.Performances.Any(p => p.Guid == performanceId);
        }

        public bool PerformanceExists(Guid directorId, Performance performance)
        {
            var director = _context.Directors.SingleOrDefault(d => d.Guid == directorId);

            return _context.Performances.Any(p => p.Title == performance.Title &&
                                            (p.PremiereDate.HasValue ? p.PremiereDate.Value.Year == performance.PremiereDate.Value.Year : true) &&
                                            p.DirectorId == director.Id);
        }

        public void AddPerformance(Guid directorId, Performance performance)
        {
            if (directorId == Guid.Empty)
                throw new ArgumentNullException(nameof(directorId));

            var director = _context.Directors.SingleOrDefault(d => d.Guid == directorId);

            if (performance == null)
                throw new ArgumentNullException(nameof(performance));

            if (PerformanceExists(directorId, performance))
                throw new ArgumentException("This performance already exists in database!");

            performance.DirectorId = director.Id;

            _context.Performances.Add(performance);
        }

        public void UpdatePerformance(Performance movie)
        {

        }

        public void DeletePerformance(Performance performance)
        {
            if (performance == null)
                throw new ArgumentNullException(nameof(performance));

            _context.Performances.Remove(performance);
        }    
    }
}
