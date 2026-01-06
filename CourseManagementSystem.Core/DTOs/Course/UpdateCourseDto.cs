// File: CourseManagementSystem.Core/DTOs/Course/UpdateCourseDto.cs

using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Core.DTOs.Course;

public class UpdateCourseDto
{
    [Required(ErrorMessage = "Category ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid category ID")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Course name is required")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 255 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount price must be greater than or equal to 0")]
    public decimal? DiscountPrice { get; set; }

    public int? InstructorId { get; set; }
}