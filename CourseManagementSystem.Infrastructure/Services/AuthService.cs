using CourseManagementSystem.Core.DTOs.Auth;
using CourseManagementSystem.Core.Entities;
using CourseManagementSystem.Core.Interfaces;
using CourseManagementSystem.Infrastructure.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Authenticate user with email and password
    /// </summary>
    /// <param name="loginDto">Login credentials containing email and password</param>
    /// <returns>Token response with JWT if authentication successful, null otherwise</returns>
    public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Find user by email
        var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

        // Return null if user doesn't exist or password is incorrect
        if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        // Generate JWT token for authenticated user
        var token = _jwtService.GenerateToken(user);
        return _jwtService.CreateTokenResponse(user, token);
    }

    /// <summary>
    /// Register a new user in the system
    /// </summary>
    /// <param name="registerDto">Registration details including credentials and role</param>
    /// <returns>Token response with JWT if registration successful, null if user already exists or invalid data</returns>
    public async Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user with email already exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return null; // Email already in use
        }

        // Check if phone number already exists
        var existingPhone = await _unitOfWork.Users.GetByPhoneNumberAsync(registerDto.PhoneNumber);
        if (existingPhone != null)
        {
            return null; // Phone number already in use
        }

        // Verify that the specified role exists
        var role = await _unitOfWork.Roles.GetByIdAsync(registerDto.RoleId);
        if (role == null)
        {
            return null; // Invalid role
        }

        // Create new user entity
        var user = new User
        {
            FullName = registerDto.FullName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            PasswordHash = _passwordService.HashPassword(registerDto.Password),
            RoleId = registerDto.RoleId,
            DateCreated = DateTime.UtcNow
        };

        // Add user to database
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Retrieve user with role information for token generation
        var userWithRole = await _unitOfWork.Users.GetUserWithRoleAsync(user.Id);
        if (userWithRole == null)
        {
            return null; // Should not happen, but handle edge case
        }

        // Generate JWT token for newly registered user
        var token = _jwtService.GenerateToken(userWithRole);
        return _jwtService.CreateTokenResponse(userWithRole, token);
    }
}
