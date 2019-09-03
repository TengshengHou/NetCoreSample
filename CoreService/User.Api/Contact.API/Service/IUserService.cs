using Contact.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Service
{
    public interface IUserService
    {
        Task<BaseUserInfo>  GetBaseUserInfoAsync(int id);
    }
}
