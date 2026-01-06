// File: CourseManagementSystem.Core/DTOs/User/UpdateUserDto.cs

using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Core.DTOs.User;

/// <summary>
/// Data transfer object for updating user information
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// User's full name
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(250, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 250 characters")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address (must be unique)
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(250, ErrorMessage = "Email cannot exceed 250 characters")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number (must be unique)
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Role ID to assign to the user
    /// </summary>
    [Required(ErrorMessage = "Role ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid role ID")]
    public int RoleId { get; set; }
}