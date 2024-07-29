using StarBucks_Backend.Models;

namespace StarBucks_Backend.Services
{
    public interface IAuthService
    {
        void SignUp(User user);
        bool Login(string userName, string password, out string role);
    }
}
