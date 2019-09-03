using Contact.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public interface IContactApplyRequestRepository
    {
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <param name="contactApplyRequest"></param>
        /// <returns></returns>
        Task<bool> AddRequestAsync(ContactApplyRequest contactApplyRequest);
        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int applierId);
        /// <summary>
        /// 好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool>  GetRequestListAsync(int userId);
    }
}
