using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Recommend.API.Data;

namespace helloApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
       
        [HttpGet("")]
        [HttpHead]
        public ActionResult Ping()
        {
            return Ok();
        }

    }
}
