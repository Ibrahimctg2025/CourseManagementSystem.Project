// File: CourseManagementSystem.Core/DTOs/User/UserDto.cs

namespace CourseManagementSystem.Core.DTOs.User;

/// <summary>
/// Data transfer object for user response
/// </summary>
public class UserDto
{
    /// <summary>
    /// User's unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// User's role ID
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// User's role name (Admin, Instructor, Student)
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when user account was created
    /// </summary>
    public DateTime DateCreated { get; set; }
}