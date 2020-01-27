using GreekTheater.API.Core.Dtos.Acting;
using GreekTheater.API.Core.Dtos.PerformanceGenre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Performance
{
    public class PerformanceForUpdateDto : PerformanceForManipulationDto
    {
        public ICollection<ActingForUpdateDto> Actings { get; set; }
            = new List<ActingForUpdateDto>();

        public ICollection<PerformanceGenreForUpdateDto> PerformanceGenres { get; set; }
            = new List<PerformanceGenreForUpdateDto>();
    }
}
