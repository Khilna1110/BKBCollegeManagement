using System; // Correct namespace for HttpGet attribute !!!!!!!!!
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BKBCollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Scripting;
using BCrypt.Net;
using Microsoft.AspNetCore.Cors;

namespace BKBCollegeManagement.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public IActionResult Register(RegisterStudent registeruser)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UserContext.Users'  is null.");
            }

            // validate
            if (_context.Users.Any(x => x.Username == registeruser.Username))
                return Problem("Username '" + registeruser.Username + "' is already taken. Registration failed");


            User user =new User();
            Student student = new Student();
            user.Username = registeruser.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registeruser.Password);
            user.Role = registeruser.Role;
            user.FullName = registeruser.FullName;
            _context.Users.Add(user);
            _context.SaveChanges();
            student.FullName = registeruser.FullName;
            student.Gender = registeruser.Gender;
            student.PhoneNumber = registeruser.PhoneNumber;
            student.CourseId = registeruser.CourseId;
            student.LastModified = DateTime.Now;
            student.UserId = user.UserId;
            student.Email = registeruser.Username;
            _context.Students.Add(student);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId },new { message = "Registration successful!" });
        }

        // POST: api/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("registerAdmin")]
        public IActionResult RegisterAdmin(RegisterAdmin
            request)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UserContext.Users'  is null.");
            }

            // validate
            if (_context.Users.Any(x => x.Username == request.Username))
                return Problem("Username '" + request.Username + "' is already taken. Registration failed");

            User user = new User();
            user.Username = request.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.FullName = request.FullName;
            user.Role = "Admin";
           
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, new { message = "Registration successful!" });
        }




        [HttpPost("login")]
        public IActionResult Authenticate(AuthenticateRequest request)
        {

            var isValidUser = _context.Users.SingleOrDefault(x => x.Username == request.Username);

            // validate
            if (isValidUser == null || !BCrypt.Net.BCrypt.Verify(request.Password, isValidUser.PasswordHash))
               return Unauthorized(new { message = "Username or password is incorrect. Login Failed" });
            
            
            
            if(isValidUser.Role=="Admin")
                return Ok(new { message = "Login successful!",username=isValidUser.Username,role=isValidUser.Role,fullName=isValidUser.FullName});
            else
            {
                var student=_context.Students.SingleOrDefault(x => x.UserId == isValidUser.UserId);
                var course= _context.Courses.SingleOrDefault(x => x.CourseId == student.CourseId);
                return Ok(new { message = "Login successful!", userID=isValidUser.UserId,username = isValidUser.Username, role = isValidUser.Role, fullName = isValidUser.FullName ,courseId=course.CourseId,courseName=course.CourseName});

            }
        }


        // GET: api/users/1/messages
        
        [HttpGet]
        [Route("{receiverId:int}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(int receiverId)
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }

            var isValidUser = _context.Users.SingleOrDefault(x => x.UserId == receiverId);

            if (isValidUser == null)
                return NotFound(new { error= "Invalid Receipient." });

            return await _context.Messages.Where( x=> x.ReceiverId== receiverId).ToListAsync();
        }

        // GET: api/users/sendMessage
        [HttpPost("sendMessage")]
        public async Task<ActionResult<Course>> SendMessage(Message message)
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }

            var isValidUser = _context.Users.SingleOrDefault(x => x.UserId == message.ReceiverId);

            if (isValidUser == null)
                return NotFound(new {error="Invalid Receipient." });

            message.CreatedDate = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMessages), new { receiverId = message.ReceiverId},new{ message = message.MessageContent });

        }

        // GET: api/users/1/course

        [HttpGet]
        [Route("{userId:int}/course")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse(int userId)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var isValidUser = _context.Students.SingleOrDefault(x => x.UserId == userId);

            if (isValidUser == null)
                return NotFound(new { error = "Invalid Receipient." });

            return await _context.Courses.Where(x => x.CourseId == isValidUser.CourseId).ToListAsync();
        }

    }
}
