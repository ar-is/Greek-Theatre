using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public class Performance
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Title { get; set; }

        public DateTimeOffset? PremiereDate { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (PremiereDate.HasValue)
                    return $"{Title}, {PremiereDate.Value.Year}";

                return Title;
            }
        }

        public int DirectorId { get; set; }
        public Director Director { get; set; }

        public ICollection<Acting> Actings { get; set; }
            = new Collection<Acting>();
        public ICollection<PerformanceGenre> PerformanceGenres { get; set; }
            = new Collection<PerformanceGenre>();
    }
}
