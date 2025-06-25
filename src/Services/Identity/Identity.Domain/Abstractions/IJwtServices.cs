using Identity.Domain.Entities;

namespace Identity.Domain.Abstractions;

public interface IJwtServices
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
    Task<string> GenerateRefreshTokenAsync();
}