using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public class PerformanceGenre
    {
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }

        public int PerformanceId { get; set; }
        public Performance Performance { get; set; }
    }
}
