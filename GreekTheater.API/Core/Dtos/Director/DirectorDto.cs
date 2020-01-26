using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Dtos.Director
{
    public class DirectorDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int? Age { get; set; }

        public List<string> Performances { get; set; }
            = new List<string>();
    }
}
