using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        public async Task<List<EntryDto>> Index([FromServices] HtmlScraper htmlScraper)
        {
            var startTime = DateTime.Now;

            var entrys = await htmlScraper.Run();

            var endTime = DateTime.Now - startTime;

            return entrys.ToList();
        }

    }
}