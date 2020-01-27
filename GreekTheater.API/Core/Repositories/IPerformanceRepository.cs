using GreekTheater.API.Core.Entities;
using GreekTheater.API.Helpers.Paging;
using GreekTheater.API.Helpers.ResourceFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Repositories
{
    public interface IPerformanceRepository
    {
        void AddPerformance(Guid directorId, Performance performance);
        void UpdatePerformance(Performance performance);
        void DeletePerformance(Performance performance);
        Performance GetPerformance(Guid performanceId);
        Performance GetPerformance(Guid directorId, Guid performanceId);
        IEnumerable<Performance> GetPerformances(Guid directorId);
        IEnumerable<Performance> GetPerformances(IEnumerable<Guid> performanceIds);
        PagedList<Performance> GetPerformances(PerformancesResourceParameters parameters);
        bool PerformanceExists(Guid performanceId);
        bool PerformanceExists(Guid directorId, Performance performance);
    }
}
