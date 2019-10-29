using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="baseUserInfo"></param>
        /// <returns></returns>
        Task<bool> UpdateContactionInfoAsync(UserIdentity baseUserInfo, CancellationToken cancellationToken);
        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="baseUserInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddContacAsync(int userId, UserIdentity baseUserInfo, CancellationToken cancellationToken);


        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<List<Models.Contact>> GetContactsAsync(int userID, CancellationToken cancellationToken);
        /// <summary>
        /// 更新好友标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<bool> TagContactAsync(int userid, int contactId, List<string> tags, CancellationToken cancellationToken);
    }
}
