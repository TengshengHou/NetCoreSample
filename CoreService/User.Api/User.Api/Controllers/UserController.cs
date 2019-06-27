using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
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

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user =await _userContext.Users.AsNoTracking().Include(u => u.Properties).SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            if (user == null)
                //return NotFound();
                throw new UserOperationException("错误的用户上下文ID");
            return Json(user) ;

        }
        [Route("")]
        [HttpPatch]
     
    public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<Model.AppUser> patch)
        {
         /*
                    {
                        "op":"replace",
                        "path":"/Company",
                        "value":"adminA"
                    } 
          */
            var user = await _userContext.Users.Include(u=>u.Properties).SingleOrDefaultAsync(u=>u.Id==UserIdentity.UserId); 
            patch.ApplyTo(user);


            foreach (var property in user.Properties)
            {
                _userContext.Entry(property).State=EntityState.Detached;
            }

            var originProperties = await _userContext.UserProperty.AsNoTracking().Where(u => u.AppUserId == UserIdentity.UserId).ToListAsync();
            var allProperties = originProperties.Union(user.Properties).Distinct();
            var removedProperties = originProperties.Except(user.Properties);
            var newProperties = allProperties.Except(originProperties);

            foreach (var property in removedProperties)
            {
                _userContext.Remove(property);
            }
            foreach (var item in newProperties)
            {
                _userContext.Add(item);
            }

            await _userContext.SaveChangesAsync();
            return Json(user);
        }
    }
}
