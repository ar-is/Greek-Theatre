using GreekTheater.API.Core.Dtos.Actor;
using GreekTheater.API.Core.Dtos.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Acting
{
    public class ActingForManipulationDto
    {
        public string RoleName { get; set; }

        public Guid ActorId { get; set; }
        public ActorDto Actor { get; set; }

        public Guid PerformanceId { get; set; }
        public PerformanceDto Performance { get; set; }
    }
}
