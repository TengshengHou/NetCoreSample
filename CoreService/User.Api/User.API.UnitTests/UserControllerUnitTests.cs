using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Api.Controllers;
using User.Api.Data;
using User.Api.Model;
using Xunit;


namespace User.API.UnitTests
{
    public class UserControllerUnitTests
    {

        private UserContext GetUserContext()
        {
#pragma warning disable CS0618 // 类型或成员已过时
            var options = new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
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

        private (UserController controller, UserContext userContext) GetUserController()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            var contorller = new UserController(context, logger);
            return (controller: contorller, userContext: context);
        }



        [Fact]
        public async Task Get_RetrunRigthUser_WithExpectedParameters()
        {
            (var contorller, var userContext) = GetUserController();
            var response = await contorller.Get();
            //Assert.IsType<JsonResult>(response);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("admin");

        }

        [Fact]
        public async Task Path_RetrunNewName_WithExpectedNewnameParameter()
        {
            (var contorller, var userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Name, "aa");
            var response = await contorller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;


            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Name.Should().Be("aa");


            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("aa");
        }


        [Fact]
        public async Task Path_RetrunNewName_WithAddProperty()
        {
            (var contorller, var userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>() {
                new UserProperty(){
                     Key="fin_industry",
                      Text="互联网",
                       Value="互联网"
                }
            });


            var response = await contorller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;


            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.First().Value.Should().Be("互联网");
            appUser.Properties.First().Key.Should().Be("fin_industry");



            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Count.Should().Be(1);
            userModel.Properties.First().Value.Should().Be("互联网");
            userModel.Properties.First().Key.Should().Be("fin_industry");
        }


        [Fact]
        public async Task Path_RetrunNewName_WithRemoveProperty()
        {
            (var contorller, var userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>() {
               
            });


            var response = await contorller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;


            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();
            



            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Should().BeEmpty();
            
        }
    }
}
