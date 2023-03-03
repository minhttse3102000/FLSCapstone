using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ITokenService
    {
        Task<RefreshToken> GetRefreshTokenByToken(string Token);
        Task<bool> DeleteRefreshToken(string Token);
    }
}
