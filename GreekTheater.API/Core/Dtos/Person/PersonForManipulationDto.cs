using GreekTheater.API.Helpers.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Person
{
    public abstract class PersonForManipulationDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DateOfBirthRange]
        public virtual DateTimeOffset? DateOfBirth { get; set; }
    }
}
