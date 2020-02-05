﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        // GET api/values
        [HttpGet("")]
        [HttpHead]
        public ActionResult Ping()
        {
            return Ok();
        }


    }
}
