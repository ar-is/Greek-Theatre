using GreekTheater.API.Core.Dtos.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Actor
{
    public class ActorForCreationWithDateOfDeathDto : ActorForCreationDto
    {
        public DateTimeOffset? DateOfDeath { get; set; }
    }
}
