// File: CourseManagementSystem.API/Controllers/CoursesController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseManagementSystem.Core.DTOs.Course;
using CourseManagementSystem.Core.Entities;
using CourseManagementSystem.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CourseManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all courses
    /// </summary>
    /// <returns>List of courses</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _unitOfWork.Courses.GetCoursesWithDetailsAsync();
        var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
        return Ok(courseDtos);
    }

    /// <summary>
    /// Get course by ID
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);

        if (course == null)
        {
            return NotFound(new { message = "Course not found" });
        }

        var courseDto = _mapper.Map<CourseDto>(course);
        return Ok(courseDto);
    }

    /// <summary>
    /// Create a new course
    /// </summary>
    /// <param name="createCourseDto">Course creation details</param>
    /// <returns>Created course</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verify category exists
        var category = await _unitOfWork.CourseCategory.GetByIdAsync(createCourseDto.CategoryId);
        if (category == null)
        {
            return BadRequest(new { message = "Invalid category" });
        }

        // Verify instructor exists if specified
        if (createCourseDto.InstructorId.HasValue)
        {
            var instructor = await _unitOfWork.Users.GetByIdAsync(createCourseDto.InstructorId.Value);
            if (instructor == null)
            {
                return BadRequest(new { message = "Invalid instructor" });
            }

            // Check if instructor has the right role
            var instructorWithRole = await _unitOfWork.Users.GetUserWithRoleAsync(createCourseDto.InstructorId.Value);
            if (instructorWithRole?.Role.RoleName != "Instructor")
            {
                return BadRequest(new { message = "Specified user is not an instructor" });
            }
        }

        // If user is instructor, they can only assign themselves
        if (User.IsInRole("Instructor"))
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            createCourseDto.InstructorId = currentUserId;
        }

        var course = _mapper.Map<Course>(createCourseDto);
        course.DateCreated = DateTime.UtcNow;
        course.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        var createdCourse = await _unitOfWork.Courses.GetCourseWithDetailsAsync(course.Id);
        var courseDto = _mapper.Map<CourseDto>(createdCourse);

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDto);
    }

    /// <summary>
    /// Update course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <param name="updateCourseDto">Course update details</param>
    /// <returns>Updated course</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
        if (course == null)
        {
            return NotFound(new { message = "Course not found" });
        }

        // Instructors can only update their own courses
        if (User.IsInRole("Instructor"))
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (course.InstructorId != currentUserId)
            {
                return Forbid("You can only update your own courses");
            }
        }

        // Verify category exists
        var category = await _unitOfWork.CourseCategory.GetByIdAsync(updateCourseDto.CategoryId);
        if (category == null)
        {
            return BadRequest(new { message = "Invalid category" });
        }

        // Verify instructor exists if specified
        if (updateCourseDto.InstructorId.HasValue)
        {
            var instructor = await _unitOfWork.Users.GetUserWithRoleAsync(updateCourseDto.InstructorId.Value);
            if (instructor == null || instructor.Role.RoleName != "Instructor")
            {
                return BadRequest(new { message = "Invalid instructor" });
            }
        }

        _mapper.Map(updateCourseDto, course);
        course.DateUpdated = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        var updatedCourse = await _unitOfWork.Courses.GetCourseWithDetailsAsync(course.Id);
        var courseDto = _mapper.Map<CourseDto>(updatedCourse);

        return Ok(courseDto);
    }

    /// <summary>
    /// Delete course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound(new { message = "Course not found" });
        }

        _unitOfWork.Courses.Remove(course);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Get courses by category
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>List of courses in specified category</returns>
    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByCategory(int categoryId)
    {
        var courses = await _unitOfWork.Courses.GetCoursesByCategoryAsync(categoryId);
        var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
        return Ok(courseDtos);
    }

    /// <summary>
    /// Get courses by instructor
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <returns>List of courses by specified instructor</returns>
    [HttpGet("by-instructor/{instructorId}")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByInstructor(int instructorId)
    {
        var courses = await _unitOfWork.Courses.GetCoursesByInstructorAsync(instructorId);
        var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
        return Ok(courseDtos);
    }

    /// <summary>
    /// Get current instructor's courses
    /// </summary>
    /// <returns>List of courses taught by current instructor</returns>
    [HttpGet("my-courses")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetMyCourses()
    {
        var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var courses = await _unitOfWork.Courses.GetCoursesByInstructorAsync(currentUserId);
        var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
        return Ok(courseDtos);
    }
}