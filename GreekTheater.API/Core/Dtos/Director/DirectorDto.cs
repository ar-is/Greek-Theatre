using GreekTheater.API.Core.Dtos.Person;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Director
{
    public class DirectorDto : PersonDto
    {
        [JsonProperty(Order = 4)]
        public List<string> Performances { get; set; }
            = new List<string>();
    }
}
