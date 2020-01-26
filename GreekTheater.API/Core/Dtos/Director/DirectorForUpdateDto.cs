using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Director
{
    public class DirectorForUpdateDto : DirectorForManipulationDto
    {
        [Required]
        public override DateTimeOffset? DateOfBirth { get => base.DateOfBirth; set => base.DateOfBirth = value; }

        //public ICollection<PerformanceForUpdateDto> Performances { get; set; }
        //    = new List<PerformanceForUpdateDto>();
    }
}
