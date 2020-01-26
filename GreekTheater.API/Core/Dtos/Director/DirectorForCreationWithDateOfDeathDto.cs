using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Director
{
    public class DirectorForCreationWithDateOfDeathDto : DirectorForCreationDto
    {
        public DateTimeOffset? DateOfDeath { get; set; }
    }
}
