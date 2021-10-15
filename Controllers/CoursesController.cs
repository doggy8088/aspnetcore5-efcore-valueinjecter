using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a1.Models;
using Omu.ValueInjecter;

namespace a1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // GET: api/Courses/5
        [HttpGet("{id}/department")]
        public async Task<ActionResult<Department>> GetCourseDepartment(int id)
        {
            var course = await _context.Courses
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return course.Department;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseUpdate course)
        {
            var item = await _context.Courses.FindAsync(id);

            // item.Title = course.Title;

            // ValueInjecter
            item.InjectFrom(course);

            // _context.Entry(course).State = EntityState.Modified;
            // _context.Update(course);

            // _context.Database.ExecuteSqlRaw("UPDATE dbo.Course SET Rating=Rating-1 WHERE CourseId=@p1 AND Rating>0", id);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /api/Courses
        [HttpDelete]
        public async Task<IActionResult> DeleteManyCourse(int[] ids)
        {
            foreach (var id in ids)
            {
                // var course = await _context.Courses.FindAsync(id);
                // if (course != null)
                // {
                //     _context.Courses.Remove(course);
                // }

                _context.Courses.Remove(new Course { CourseId = id });
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /api/Courses/byDept/{id}
        [HttpDelete("byDept/{id}")]
        public async Task<IActionResult> DeleteByDepartment(int id)
        {
            var data = await _context.Courses.Where(p => p.DepartmentId == id).ToListAsync();

            _context.Courses.RemoveRange(data);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
