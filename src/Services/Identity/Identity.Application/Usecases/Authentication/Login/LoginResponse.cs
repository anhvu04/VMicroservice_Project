namespace Identity.Application.Usecases.Authentication.Login;

public class LoginResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}