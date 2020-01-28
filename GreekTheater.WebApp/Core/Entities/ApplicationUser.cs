using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.WebApp.Core.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public Guid Guid { get; private set; } = Guid.NewGuid();
    }
}
