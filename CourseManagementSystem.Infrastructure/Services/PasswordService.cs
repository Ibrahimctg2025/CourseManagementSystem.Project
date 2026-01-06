using CourseManagementSystem.Infrastructure.Services.Interfaces;

public class PasswordService : IPasswordService
{
    /// <summary>
    /// Hash a password using BCrypt with automatic salt generation
    /// BCrypt automatically generates a unique salt for each password
    /// </summary>
    /// <param name="password">Plain text password to hash</param>
    /// <returns>BCrypt hashed password including salt</returns>
    public string HashPassword(string password)
    {
        // BCrypt.HashPassword automatically includes salt in the hash
        // Default work factor is 11, which provides good security
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verify a password against its BCrypt hash
    /// BCrypt extracts the salt from the hash and verifies the password
    /// </summary>
    /// <param name="password">Plain text password to verify</param>
    /// <param name="hashedPassword">BCrypt hashed password with embedded salt</param>
    /// <returns>True if the password is correct, false otherwise</returns>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            // BCrypt.Verify automatically extracts salt from hashedPassword
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception)
        {
            // Return false if verification fails due to invalid hash format
            return false;
        }
    }
}
