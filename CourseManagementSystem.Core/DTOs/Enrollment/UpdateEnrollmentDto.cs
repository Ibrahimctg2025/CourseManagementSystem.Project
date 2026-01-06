
using System.ComponentModel.DataAnnotations;
using CourseManagementSystem.Core.Enums;

namespace CourseManagementSystem.Core.DTOs.Enrollment;

public class UpdateEnrollmentDto
{
    [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Payment amount is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Payment amount must be greater than or equal to 0")]
    public decimal PaymentAmount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
    public decimal Discount { get; set; }

    public DateTime? PaymentDate { get; set; }

    [Required(ErrorMessage = "Enrollment status is required")]
    public EnrollmentStatus EnrollmentStatus { get; set; }

    [Required(ErrorMessage = "Payment status is required")]
    public PaymentStatus PaymentStatus { get; set; }
}