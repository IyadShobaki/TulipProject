using System.Threading.Tasks;
using TulipWpfUI.Models;

namespace TulipWpfUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}