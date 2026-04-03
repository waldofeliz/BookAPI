using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
