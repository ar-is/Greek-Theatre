using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public class Acting
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string RoleName { get; set; }

        public string DisplayTitle => $"{RoleName}, {Performance.DisplayTitle}";

        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public int PerformanceId { get; set; }
        public Performance Performance { get; set; }
    }
}
