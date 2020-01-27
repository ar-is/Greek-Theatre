using GreekTheater.API.Core.Dtos.Acting;
using GreekTheater.API.Core.Dtos.PerformanceGenre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Performance
{
    public class PerformanceForCreationDto : PerformanceForManipulationDto
    {
        public ICollection<ActingForCreationDto> Actings { get; set; }
            = new List<ActingForCreationDto>();

        public ICollection<PerformanceGenreForCreationDto> PerformanceGenres { get; set; }
            = new List<PerformanceGenreForCreationDto>();
    }
}
