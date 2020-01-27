using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.ResourceFilters
{
    public class PerformancesResourceParameters : ResourceParameters
    {
        [FromQuery(Name = "")]
        public PerformancesFilter Filter { get; set; }

        public override string OrderBy { get; set; } = "Director";
    }
}
