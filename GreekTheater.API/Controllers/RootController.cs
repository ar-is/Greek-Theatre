using GreekTheater.API.Core.Services.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly IRootPaginationService _linksService;

        public RootController(IRootPaginationService linksService)
        {
            _linksService = linksService ?? throw new ArgumentNullException(nameof(linksService));
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            return Ok(_linksService.CreateRootLinks());
        }
    }
}
