using CourseManagementSystem.Core.DTOs.Auth;



namespace CourseManagementSystem.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate a user with email and password
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>Token response if successful, null if authentication fails</returns>
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Register a new user in the system
        /// </summary>
        /// <param name="registerDto">Registration information</param>
        /// <returns>Token response if successful, null if registration fails</returns>
        Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto);
    }

}