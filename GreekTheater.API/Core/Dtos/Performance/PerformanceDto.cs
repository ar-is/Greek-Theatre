using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Performance
{
    public class PerformanceDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PremiereDate { get; set; }

        public string Director { get; set; }

        public List<string> Actors { get; set; }
            = new List<string>();

        public List<string> Genres { get; set; }
            = new List<string>();
    }
}
