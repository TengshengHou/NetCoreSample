using Microsoft.AspNetCore.Mvc;

namespace Contact.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity() { UserId = 1};


    }
}
