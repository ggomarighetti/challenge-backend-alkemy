using Challenge.Model;
using System.Threading.Tasks;

namespace Challenge.Server.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<User> GetUser(int identifier);
        Task<User> Login(Authentication credentials);
        Task<User> Register(Authentication credentials);

        // Utility
        Task<bool> UserExists(Authentication credentials);
    }
}
