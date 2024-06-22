using System.Security.Cryptography;

namespace FinanceManager.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginModel loginModel);
    Task RegisterAsync(RegisterModel registerModel);
}

public class AuthService(
    IUserRepository userRepository,
    IJwtUtils jwtUtils,
    ILogger<AuthService> logger
    ) : IAuthService
{
    public async Task<string> LoginAsync(LoginModel loginModel)
    {
        var user = await userRepository.GetUserByUsernameAsync(loginModel.Username);
        if (user == null || !VerifyPasswordHash(loginModel.Password, user.PasswordHash))
            throw new ApplicationException("Username or password is incorrect");

        return jwtUtils.GenerateToken(user);
    }

    public async Task RegisterAsync(RegisterModel registerModel)
    {
        if (await userRepository.GetUserByUsernameAsync(registerModel.Username) != null)
            throw new ApplicationException("Username already taken");

        var user = new User
        {
            UserName = registerModel.Username,
            Email = registerModel.Email,
            DateCreated = DateTime.UtcNow,
            PasswordHash = HashPassword(registerModel.Password)
        };

        await userRepository.AddUserAsync(user);

        logger.LogInformation("AuthenticationService.RegisterAsync. User registered successfully : {@user}",user);
    }

    private static string HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }

    private static bool VerifyPasswordHash(string password, string storedHash)
    {
        using var hmac = new HMACSHA512();
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var storedHashBytes = Convert.FromBase64String(storedHash);
        return hash.SequenceEqual(storedHashBytes);
    }
}
