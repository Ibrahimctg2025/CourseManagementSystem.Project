
using Microsoft.EntityFrameworkCore;
using CourseManagementSystem.Core.Entities;
using CourseManagementSystem.Core.Enums;
using CourseManagementSystem.Infrastructure.Services;

namespace CourseManagementSystem.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Seed Roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new Role
                {
                    RoleName = "Admin",
                    Description = "System Administrator with full access"
                },
                new Role
                {
                    RoleName = "Instructor",
                    Description = "Course Instructor who can create and manage courses"
                },
                new Role
                {
                    RoleName = "Student",
                    Description = "Student who can enroll in courses"
                }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Seed Admin User
        if (!await context.Users.AnyAsync(u => u.Email == "admin"))
        {
            var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Admin");
            var passwordService = new PasswordService();

            var adminUser = new User
            {
                FullName = "System Administrator",
                Email = "admin",
                PhoneNumber = "1234567890",
                PasswordHash = passwordService.HashPassword("admin123"),
                RoleId = adminRole.Id,
                DateCreated = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }

        // Seed Course Categories
        if (!await context.CourseCategory.AnyAsync())
        {
            var categories = new List<CourseCategory>
            {
                new CourseCategory
                {
                    Name = "Programming",
                    Description = "Programming and software development courses"
                },
                new CourseCategory
                {
                    Name = "Web Development",
                    Description = "Web development and design courses"
                },
                new CourseCategory
                {
                    Name = "Mobile Development",
                    Description = "Mobile app development courses for iOS and Android"
                },
                new CourseCategory
                {
                    Name = "Data Science",
                    Description = "Data science, analytics, and machine learning courses"
                },
                new CourseCategory
                {
                    Name = "DevOps",
                    Description = "DevOps, CI/CD, and cloud computing courses"
                },
                new CourseCategory
                {
                    Name = "Cybersecurity",
                    Description = "Information security and cybersecurity courses"
                },
                new CourseCategory
                {
                    Name = "Database",
                    Description = "Database design, administration, and management courses"
                }
            };

            await context.CourseCategory.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // Seed Sample Instructors
        if (!await context.Users.AnyAsync(u => u.Role.RoleName == "Instructor"))
        {
            var instructorRole = await context.Roles.FirstAsync(r => r.RoleName == "Instructor");
            var passwordService = new PasswordService();

            var instructors = new List<User>
            {
                new User
                {
                    FullName = "John Smith",
                    Email = "john.smith@example.com",
                    PhoneNumber = "1234567891",
                    PasswordHash = passwordService.HashPassword("instructor123"),
                    RoleId = instructorRole.Id,
                    DateCreated = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Jane Doe",
                    Email = "jane.doe@example.com",
                    PhoneNumber = "1234567892",
                    PasswordHash = passwordService.HashPassword("instructor123"),
                    RoleId = instructorRole.Id,
                    DateCreated = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Michael Johnson",
                    Email = "michael.johnson@example.com",
                    PhoneNumber = "1234567893",
                    PasswordHash = passwordService.HashPassword("instructor123"),
                    RoleId = instructorRole.Id,
                    DateCreated = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(instructors);
            await context.SaveChangesAsync();
        }

        // Seed Sample Courses
        if (!await context.Courses.AnyAsync())
        {
            var programmingCategory = await context.CourseCategory.FirstAsync(c => c.Name == "Programming");
            var webDevCategory = await context.CourseCategory.FirstAsync(c => c.Name == "Web Development");
            var mobileCategory = await context.CourseCategory.FirstAsync(c => c.Name == "Mobile Development");
            var dataScienceCategory = await context.CourseCategory.FirstAsync(c => c.Name == "Data Science");

            var instructor1 = await context.Users.FirstAsync(u => u.Email == "john.smith@example.com");
            var instructor2 = await context.Users.FirstAsync(u => u.Email == "jane.doe@example.com");
            var instructor3 = await context.Users.FirstAsync(u => u.Email == "michael.johnson@example.com");

            var courses = new List<Course>
            {
                new Course
                {
                    Name = "C# Fundamentals",
                    Description = "Learn the basics of C# programming language including variables, data types, control structures, and object-oriented programming concepts.",
                    Price = 299.99m,
                    DiscountPrice = 199.99m,
                    CategoryId = programmingCategory.Id,
                    InstructorId = instructor1.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new Course
                {
                    Name = "ASP.NET Core Web API",
                    Description = "Build modern, scalable web APIs with ASP.NET Core. Learn about RESTful services, authentication, and best practices.",
                    Price = 399.99m,
                    DiscountPrice = 299.99m,
                    CategoryId = webDevCategory.Id,
                    InstructorId = instructor2.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new Course
                {
                    Name = "JavaScript for Beginners",
                    Description = "Master JavaScript from scratch. Learn ES6+ features, DOM manipulation, async programming, and modern JavaScript development.",
                    Price = 199.99m,
                    DiscountPrice = 149.99m,
                    CategoryId = webDevCategory.Id,
                    InstructorId = instructor1.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new Course
                {
                    Name = "React Native Mobile Development",
                    Description = "Build cross-platform mobile apps for iOS and Android using React Native. Learn components, navigation, and state management.",
                    Price = 449.99m,
                    DiscountPrice = 349.99m,
                    CategoryId = mobileCategory.Id,
                    InstructorId = instructor3.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new Course
                {
                    Name = "Python for Data Science",
                    Description = "Learn Python programming for data analysis and machine learning. Covers NumPy, Pandas, Matplotlib, and scikit-learn.",
                    Price = 499.99m,
                    DiscountPrice = 399.99m,
                    CategoryId = dataScienceCategory.Id,
                    InstructorId = instructor2.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new Course
                {
                    Name = "Advanced C# Programming",
                    Description = "Deep dive into advanced C# topics including LINQ, async/await, delegates, events, and design patterns.",
                    Price = 349.99m,
                    CategoryId = programmingCategory.Id,
                    InstructorId = instructor1.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }

        // Seed Sample Students
        if (!await context.Users.AnyAsync(u => u.Role.RoleName == "Student"))
        {
            var studentRole = await context.Roles.FirstAsync(r => r.RoleName == "Student");
            var passwordService = new PasswordService();

            var students = new List<User>
            {
                new User
                {
                    FullName = "Alice Johnson",
                    Email = "alice.johnson@example.com",
                    PhoneNumber = "1234567894",
                    PasswordHash = passwordService.HashPassword("student123"),
                    RoleId = studentRole.Id,
                    DateCreated = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Bob Williams",
                    Email = "bob.williams@example.com",
                    PhoneNumber = "1234567895",
                    PasswordHash = passwordService.HashPassword("student123"),
                    RoleId = studentRole.Id,
                    DateCreated = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(students);
            await context.SaveChangesAsync();

            // Add sample enrollments
            var course1 = await context.Courses.FirstAsync(c => c.Name == "C# Fundamentals");
            var course2 = await context.Courses.FirstAsync(c => c.Name == "JavaScript for Beginners");
            var student1 = students[0];
            var student2 = students[1];

            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    CourseId = course1.Id,
                    UserId = student1.Id,
                    Description = "Enrolled in C# Fundamentals course",
                    PaymentAmount = 199.99m,
                    Discount = 0,
                    EnrollmentDate = DateTime.UtcNow,
                    PaymentDate = DateTime.UtcNow,
                    EnrollmentStatus = EnrollmentStatus.Enrolled,
                    PaymentStatus = PaymentStatus.Paid
                },
                new Enrollment
                {
                    CourseId = course2.Id,
                    UserId = student1.Id,
                    Description = "Enrolled in JavaScript for Beginners course",
                    PaymentAmount = 149.99m,
                    Discount = 0,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-5),
                    PaymentDate = DateTime.UtcNow.AddDays(-5),
                    EnrollmentStatus = EnrollmentStatus.Enrolled,
                    PaymentStatus = PaymentStatus.Paid
                },
                new Enrollment
                {
                    CourseId = course1.Id,
                    UserId = student2.Id,
                    Description = "Enrolled in C# Fundamentals course",
                    PaymentAmount = 199.99m,
                    Discount = 20.00m,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-2),
                    PaymentDate = null,
                    EnrollmentStatus = EnrollmentStatus.Processing,
                    PaymentStatus = PaymentStatus.Pending
                }
            };

            await context.Enrollments.AddRangeAsync(enrollments);
            await context.SaveChangesAsync();
        }
    }
}