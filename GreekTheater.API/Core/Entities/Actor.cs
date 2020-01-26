using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public class Actor : Person
    {
        public ICollection<Acting> Actings { get; set; }
            = new Collection<Acting>();
    }
}
