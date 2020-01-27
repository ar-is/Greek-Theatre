using GreekTheater.API.Core.Dtos.Genre;
using GreekTheater.API.Core.Dtos.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.PerformanceGenre
{
    public class PerformanceGenreForManipulationDto
    {
        public Guid GenreId { get; set; }
        public GenreDto Genre { get; set; }

        public Guid PerformanceId { get; set; }
        public PerformanceDto Performance { get; set; }
    }
}
