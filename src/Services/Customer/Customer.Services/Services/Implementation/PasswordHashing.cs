using Customer.Services.Services.Interfaces;

namespace Customer.Services.Services.Implementation;

public class PasswordHashing : IPasswordHashing
{
    public async Task<string> HashPasswordAsync(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hashedPassword));
    }
}