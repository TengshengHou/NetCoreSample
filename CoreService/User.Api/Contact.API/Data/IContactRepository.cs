using Contact.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<bool> UpdateContactionInfo(BaseUserInfo baseUserInfo);
    }
}
