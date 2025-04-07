using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public long ClientId { get; set; }
}
