using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Actor
{
    public class ActorForUpdateDto : ActorForManipulationDto
    {
        [Required]
        public override DateTimeOffset? DateOfBirth { get => base.DateOfBirth; set => base.DateOfBirth = value; }
    }
}
