using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public class Genre
    {
        public byte Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public ICollection<PerformanceGenre> PerformanceGenres { get; set; }
            = new Collection<PerformanceGenre>();
    }
}
