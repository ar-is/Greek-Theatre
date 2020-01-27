using GreekTheater.API.Core.Dtos.Links;
using GreekTheater.API.Core.Services.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Services.Pagination
{
    public class RootPaginationService : IRootPaginationService
    {
        private readonly IUrlHelper _urlHelper;

        public RootPaginationService(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        public IEnumerable<LinkDto> CreateRootLinks()
        {
            return new List<LinkDto>
            {
                new LinkDto(_urlHelper.Link("GetRoot", new { }),
                    "self",
                    "GET"),

                new LinkDto(_urlHelper.Link("GetDirectors", new { }),
                    "directors",
                    "GET"),

                new LinkDto(_urlHelper.Link("CreateDirector", new { }),
                    "create_director",
                    "POST"),

                new LinkDto(_urlHelper.Link("GetActors", new { }),
                    "actors",
                    "GET"),

                new LinkDto(_urlHelper.Link("CreateActor", new { }),
                    "create_actor",
                    "POST"),

                new LinkDto(_urlHelper.Link("GetPerformances", new { }),
                    "performances",
                    "GET")
            };
        }
    }
}
