using GreekTheater.API.Core.Dtos.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Services.Pagination
{
    public interface IRootPaginationService
    {
        IEnumerable<LinkDto> CreateRootLinks();
    }
}
