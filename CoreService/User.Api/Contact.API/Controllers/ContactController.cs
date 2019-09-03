using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Dtos;
using Contact.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : BaseController
    {
        IContactApplyRequestRepository _contactApplyRequestRepository;
        IUserService _userService;
        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository, IUserService userService)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
        }

        /// <summary>
        /// 好友申请
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult> GetApplyReqeusts()
        {
            var request = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId);
            return Ok();
        }
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult> AddApplyRequestAsync(int userid)
        {
            BaseUserInfo baseUserInfo = await _userService.GetBaseUserInfoAsync(userid);
            if (baseUserInfo == null)
            {
                throw new Exception("用户参数错误");
            }
            var result = await _contactApplyRequestRepository.AddRequestAsync(new Models.ContactApplyRequest()
            {
                UserId = userid,
                ApplierID = UserIdentity.UserId,
                Name = baseUserInfo.Name,
                Company = baseUserInfo.Company,
                Title = baseUserInfo.Title,
                CreateTime = DateTime.Now,
                Avatar = baseUserInfo.Avatar
            });

            if (!result)
                return BadRequest();
            return Ok();
        }
        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult> AddApplyRequest(int applierId)
        {

            var result = await _contactApplyRequestRepository.ApprovalAsync(applierId);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
