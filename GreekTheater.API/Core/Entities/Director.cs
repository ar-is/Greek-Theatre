using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public class Director : Person
    {
        public ICollection<Performance> Performances { get; set; }
            = new Collection<Performance>();
    }
}
