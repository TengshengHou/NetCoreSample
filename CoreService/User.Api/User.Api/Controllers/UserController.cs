﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Api.Data;
namespace User.Api.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : BaseController
    {
        UserContext _userContext;
        public UserController(UserContext userContext) 
        {
            _userContext = userContext;
        }

        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user =await _userContext.Users.AsNoTracking().Include(u => u.Properties).SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            if (user == null)
                //return NotFound();
                throw new UserOperationException("错误的用户上下文ID");
            return Json(user) ;

        }
    }
}
