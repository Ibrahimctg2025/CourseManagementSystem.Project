
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseManagementSystem.Core.DTOs.CourseCategory;
using CourseManagementSystem.Core.Entities;
using CourseManagementSystem.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CourseManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseCategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all course categories
    /// </summary>
    /// <returns>List of course categories</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseCategoryDto>>> GetCourseCategory()
    {
        var categories = await _unitOfWork.CourseCategory.GetCategoriesWithCoursesAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CourseCategoryDto>>(categories);
        return Ok(categoryDtos);
    }

    /// <summary>
    /// Get course category by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Course category details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseCategoryDto>> GetCourseCategory(int id)
    {
        var category = await _unitOfWork.CourseCategory.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound(new { message = "Course category not found" });
        }

        var categoryDto = _mapper.Map<CourseCategoryDto>(category);
        return Ok(categoryDto);
    }

    /// <summary>
    /// Create a new course category
    /// </summary>
    /// <param name="createCategoryDto">Category creation details</param>
    /// <returns>Created course category</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourseCategoryDto>> CreateCourseCategory([FromBody] CreateCourseCategoryDto createCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if category with same name already exists
        var existingCategory = await _unitOfWork.CourseCategory.GetByNameAsync(createCategoryDto.Name);
        if (existingCategory != null)
        {
            return BadRequest(new { message = "Category with this name already exists" });
        }

        var category = _mapper.Map<CourseCategory>(createCategoryDto);
        await _unitOfWork.CourseCategory.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var categoryDto = _mapper.Map<CourseCategoryDto>(category);
        return CreatedAtAction(nameof(GetCourseCategory), new { id = category.Id }, categoryDto);
    }

    /// <summary>
    /// Update course category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="updateCategoryDto">Category update details</param>
    /// <returns>Updated course category</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourseCategoryDto>> UpdateCourseCategory(int id, [FromBody] UpdateCourseCategoryDto updateCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = await _unitOfWork.CourseCategory.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound(new { message = "Course category not found" });
        }

        // Check if name is being changed and if it already exists
        if (category.Name != updateCategoryDto.Name)
        {
            var existingCategory = await _unitOfWork.CourseCategory.GetByNameAsync(updateCategoryDto.Name);
            if (existingCategory != null)
            {
                return BadRequest(new { message = "Category with this name already exists" });
            }
        }

        _mapper.Map(updateCategoryDto, category);
        _unitOfWork.CourseCategory.Update(category);
        await _unitOfWork.SaveChangesAsync();

        var categoryDto = _mapper.Map<CourseCategoryDto>(category);
        return Ok(categoryDto);
    }

    /// <summary>
    /// Delete course category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourseCategory(int id)
    {
        var category = await _unitOfWork.CourseCategory.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound(new { message = "Course category not found" });
        }

        // Check if category has courses
        var courses = await _unitOfWork.Courses.GetCoursesByCategoryAsync(id);
        if (courses.Any())
        {
            return BadRequest(new { message = "Cannot delete category that has courses. Please move or delete courses first." });
        }

        _unitOfWork.CourseCategory.Remove(category);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}