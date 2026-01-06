// File: CourseManagementSystem.Core/DTOs/CourseCategory/CreateCourseCategoryDto.cs

using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Core.DTOs.CourseCategory;

public class CreateCourseCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; }
}