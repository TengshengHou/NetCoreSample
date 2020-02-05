using Recommend.API.Dtos;
using System.Threading.Tasks;

namespace Recommend.API.Service
{
    public interface IUserService
    {
        Task<UserIdentity> GetBaseUserInfoAsync(int UserId);
    }
}
