using Contact.API.Data;
using Contact.API.Service;
using Contact.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//验证是否登录
    public class ContactController : BaseController
    {
        IContactApplyRequestRepository _contactApplyRequestRepository;
        private readonly IUserService _userService;
        IContactRepository _contactRepository;
        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository, IUserService userService, IContactRepository contactRepository)
        //public ContactController(IContactApplyRequestRepository contactApplyRequestRepository, IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var v = await _contactRepository.GetContactsAsync(UserIdentity.UserId, cancellationToken);
            return Ok(v);
        }



        /// <summary>
        /// 好友申请
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("tag")]
        public async Task<IActionResult> TagContact([FromBody]TagContactInputViewModel vieModel, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.TagContactAsync(UserIdentity.UserId, vieModel.ContactId, vieModel.Tags, cancellationToken);
            if (result)
                return Ok();
            return BadRequest();
        }










        /// <summary>
        /// 获取好友申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-request")]
        public async Task<IActionResult> GetApplyReqeusts(CancellationToken cancellationToken)
        {
            var request = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId, cancellationToken);
            return Ok(request);
        }

        ///// <summary>
        ///// 好友申请
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("apply-request")]

        //public async Task<IActionResult> Get(CancellationToken cancellationToken)
        //{
        //    var request = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId, cancellationToken);
        //    return Ok(request);
        //}

        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request/{userId}")]
        public async Task<IActionResult> AddApplyRequestAsync(int userId, CancellationToken cancellationToken)
        {
            //BaseUserInfo baseUserInfo = await _userService.GetBaseUserInfoAsync(userid);
            //if (baseUserInfo == null)
            //{
            //    throw new Exception("用户参数错误");
            //}
            var result = await _contactApplyRequestRepository.AddRequestAsync(new Models.ContactApplyRequest()
            {
                UserId = userId,
                ApplierID = UserIdentity.UserId,
                Name = UserIdentity.Name,
                Company = UserIdentity.Company,
                Title = UserIdentity.Title,
                CreateTime = DateTime.Now,
                Avatar = UserIdentity.Avatar
            }, cancellationToken);

            if (!result)
                return BadRequest();
            return Ok();
        }
        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("apply-request")]
        public async Task<IActionResult> AddApplyRequest([FromForm]int applierId, CancellationToken cancellationToken)
        {

            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId, cancellationToken);
            if (!result)
                return BadRequest();
            UserIdentity applier = await _userService.GetBaseUserInfoAsync(applierId);
            var userInfo = await _userService.GetBaseUserInfoAsync(UserIdentity.UserId);
            await _contactRepository.AddContacAsync(UserIdentity.UserId, applier, cancellationToken);
            await _contactRepository.AddContacAsync(applierId, userInfo, cancellationToken);
            return Ok();
        }
    }
}

