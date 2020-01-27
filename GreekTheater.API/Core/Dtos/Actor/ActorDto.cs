using GreekTheater.API.Core.Dtos.Person;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Actor
{
    public class ActorDto : PersonDto
    {
        [JsonProperty(Order = 4)]
        public List<string> Actings { get; set; }
            = new List<string>();
    }
}
