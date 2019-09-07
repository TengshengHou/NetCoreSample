using Contact.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Task<bool> AddRequestAsync(ContactApplyRequest request, CancellationToken cancellationToken);
        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken);
        /// <summary>
        /// 好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken);
    }
}
