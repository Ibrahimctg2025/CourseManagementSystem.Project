
using AutoMapper;
using CourseManagementSystem.Core.DTOs.Course;
using CourseManagementSystem.Core.DTOs.CourseCategory;
using CourseManagementSystem.Core.DTOs.Enrollment;
using CourseManagementSystem.Core.DTOs.User;
using CourseManagementSystem.Core.Entities;

namespace CourseManagementSystem.Infrastructure.Mappings;

/// <summary>
/// AutoMapper profile configuration for entity-to-DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ============================================
        // USER MAPPINGS
        // ============================================

        // Map User entity to UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role.RoleName));

        // Map CreateUserDto to User entity
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.InstructorCourses, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        // Map UpdateUserDto to User entity
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.InstructorCourses, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        // ============================================
        // COURSE MAPPINGS
        // ============================================

        // Map Course entity to CourseDto
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.InstructorName,
                opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.FullName : null));

        // Map CreateCourseDto to Course entity
        CreateMap<CreateCourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Instructor, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        // Map UpdateCourseDto to Course entity
        CreateMap<UpdateCourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Instructor, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        // ============================================
        // ENROLLMENT MAPPINGS
        // ============================================

        // Map Enrollment entity to EnrollmentDto
        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.FullName));

        // Map CreateEnrollmentDto to Enrollment entity
        CreateMap<CreateEnrollmentDto, Enrollment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentTotal, opt => opt.Ignore())
            .ForMember(dest => dest.EnrollmentDate, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // Map UpdateEnrollmentDto to Enrollment entity
        CreateMap<UpdateEnrollmentDto, Enrollment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CourseId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentTotal, opt => opt.Ignore())
            .ForMember(dest => dest.EnrollmentDate, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // ============================================
        // COURSE CATEGORY MAPPINGS
        // ============================================

        // Map CourseCategory entity to CourseCategoryDto
        CreateMap<CourseCategory, CourseCategoryDto>()
            .ForMember(dest => dest.CourseCount,
                opt => opt.MapFrom(src => src.Courses.Count));

        // Map CreateCourseCategoryDto to CourseCategory entity
        CreateMap<CreateCourseCategoryDto, CourseCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        // Map UpdateCourseCategoryDto to CourseCategory entity
        CreateMap<UpdateCourseCategoryDto, CourseCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Courses, opt => opt.Ignore());
    }
}