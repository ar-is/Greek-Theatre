using GreekTheater.API.Core.Dtos.Director;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.CustomValidations
{
    public class DateOfBirthRange : ValidationAttribute
    {
        private static readonly DateTime MinimumDateOfBirth = new DateTime(1800, 01, 01);
        private static readonly DateTime MaximumDateOfBirth = DateTime.Now;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var director = (DirectorForManipulationDto)validationContext.ObjectInstance;

            if (director.DateOfBirth.HasValue && !DateOfBirthHasValidRange())
                return new ValidationResult(
                    $"Date of Birth must be between {MinimumDateOfBirth.Year} and {MaximumDateOfBirth.Year}",
                    new[] { nameof(DirectorForManipulationDto) });

            return ValidationResult.Success;

            bool DateOfBirthHasValidRange()
                => director.DateOfBirth >= MinimumDateOfBirth &&
                   director.DateOfBirth <= MaximumDateOfBirth;
        }
    }
}
