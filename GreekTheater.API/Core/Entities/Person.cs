using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Entities
{
    public abstract class Person
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }
        public DateTimeOffset? DateOfDeath { get; set; }
    }
}
