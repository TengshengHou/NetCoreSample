using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recommend.Api.Controllers;
using Recommend.API.Data;
using Recommend.API.Service;

namespace Recommend.API.Controllers
{
    [Route("api/Recommends")]
    [ApiController]
    public class RecommendController : BaseController
    {
        private RecommendDbContext _recommendDbContext;

        public RecommendController(RecommendDbContext recommendDbContext)
        {
            _recommendDbContext = recommendDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromServicesAttribute] IUserService userService)
        {
            //var v = userService.GetBaseUserInfoAsync(2);
            var ret = await _recommendDbContext.Recommends.AsNoTracking().Where(r => r.UserId == UserIdentity.UserId).ToListAsync();
            return Ok(ret);

        }
    }
}
