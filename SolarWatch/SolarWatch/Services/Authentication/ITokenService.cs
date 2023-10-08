using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication;

public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}
