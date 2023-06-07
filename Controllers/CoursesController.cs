using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BKBCollegeManagement.Models;

namespace BKBCollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly UserContext _context;

        public CoursesController(UserContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            return await _context.Courses.ToListAsync();
        }

        // GET: api/courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(long id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

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

        // POST: api/courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'UserContext.Courses'  is null.");
            }
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(long id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var todoItem = await _context.Courses.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(long id)
        {
            return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }


        //api/courses/1/students
        [HttpGet]
        [Route("{courseId:int}/students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetUsersForCourse(int courseId)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var isValidCourse = _context.Courses.SingleOrDefault(x => x.CourseId == courseId);

            if (isValidCourse == null)
                return NotFound(new { error = "Invalid Course." });

            return await _context.Students.Where(x => x.CourseId == courseId).ToListAsync();
        }

        //api/courses/1/subjects
        [HttpGet]
        [Route("{courseId:int}/subjects")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsForCourse(int courseId)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var isValidCourse = _context.Courses.SingleOrDefault(x => x.CourseId == courseId);

            if (isValidCourse == null)
                return NotFound(new { error = "Invalid Course." });

            return await _context.Subjects.Where(x => x.CourseId == courseId).ToListAsync();
        }


        // POST: api/courses/1/addSubject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("addSubject")]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'UserContext.Subjects'  is null.");
            }
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubject), new{id=subject.SubjectId}, subject);
        }

        // GET: api/courses
        [HttpGet("GetSubject")]
        [ActionName(nameof(GetSubject))]
        public async Task<ActionResult<Subject>> GetSubject(int subjectId)
        {
            if (_context.Subjects == null)
            {
                return NotFound();
            }
            var subject = await _context.Subjects.FindAsync(subjectId);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }
    }
}
