using Microsoft.AspNetCore.Identity;

namespace Tracksesh.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
