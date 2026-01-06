
namespace CourseManagementSystem.Core.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    ICourseCategoryRepository CourseCategory { get; }
    IRoleRepository Roles { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}