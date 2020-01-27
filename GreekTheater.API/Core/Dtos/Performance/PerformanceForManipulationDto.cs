using GreekTheater.API.Core.Dtos.Acting;
using GreekTheater.API.Core.Dtos.PerformanceGenre;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Performance
{
    public class PerformanceForManipulationDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public DateTimeOffset? PremiereDate { get; set; }
    }
}
