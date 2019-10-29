using System.Threading.Tasks;

namespace Contact.API.Service
{
    public interface IUserService
    {
        Task<UserIdentity> GetBaseUserInfoAsync(int UserId);
    }
}
