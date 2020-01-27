using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Performance
{
    public class PerformanceFullDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? PremiereDate { get; set; }

        public Guid DirectorId { get; set; }
    }
}
