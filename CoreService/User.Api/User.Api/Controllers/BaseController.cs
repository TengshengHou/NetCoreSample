using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Api.Dtos;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity() { UserId = 1, Name = "2222" };


    }
}
