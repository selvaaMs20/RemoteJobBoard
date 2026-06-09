using RemoteJobBoard.Core.Entities;

namespace RemoteJobBoard.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
