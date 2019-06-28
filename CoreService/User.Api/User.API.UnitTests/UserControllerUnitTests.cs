using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using User.Api.Controllers;
using User.Api.Data;
using Xunit;

namespace User.API.UnitTests
{
    public class UserControllerUnitTests
    {

        private UserContext GetUserContext() {
#pragma warning disable CS0618 // 类型或成员已过时
            var options = new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase().Options;
#pragma warning restore CS0618 // 类型或成员已过时

            var userContext = new UserContext(options);
            userContext.Users.Add(new Api.Model.AppUser
            {
                Id = 1,
                Name = "admin"
            });
            userContext.SaveChanges();



            return userContext;
        }


        [Fact]
        public async Task Get_RetrunRigthUser_WithExpectedParameters()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            var contorller = new UserController(context, logger);
            var response = await contorller.Get();
            Assert.IsType<JsonResult>(response);
        }
    }
}
