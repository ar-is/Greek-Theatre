using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.VendorMediaTypes
{
    public class MediaTypes
    {
        // General
        public const string Json = "application/json";
        public const string ProblemPlusJson = "application/problem+json";
        public const string HateoasPlusJson = "application/vnd.marvin.hateoas+json";

        // Movie
        public const string PrimaryFullPerformance = "vnd.marvin.performance.full";
        public const string FullPerformanceJson = "application/vnd.marvin.performance.full+json";
        public const string FullPerformanceHateoasPlusJson = "application/vnd.marvin.performance.full.hateoas+json";
        public const string FriendlyPerformanceJson = "application/vnd.marvin.performance.friendly+json";
        public const string FriendlyPerformanceHateoasPlusJson = "application/vnd.marvin.performance.friendly.hateoas+json";
    }
}
