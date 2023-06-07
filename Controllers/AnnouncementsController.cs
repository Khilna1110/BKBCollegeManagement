using BKBCollegeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BKBCollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly UserContext _context;

        public AnnouncementsController(UserContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAnnouncements()
        {
            if (_context.Announcements == null)
            {
                return NotFound();
            }
            return await _context.Announcements.ToListAsync();
        }

        // GET: api/announcemets/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(int id)
        {
            if (_context.Announcements == null)
            {
                return NotFound();
            }
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null)
            {
                return NotFound();
            }

            return announcement;
        }


        // POST: api/Announcements
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostAnnouncement(Announcement announcement)
        {
            if (_context.Announcements == null)
            {
                return Problem("Entity set 'UserContext.Announcements'  is null.");
            }
            announcement.PostedAt= DateTime.Now;
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnnouncement), new { id = announcement.AnnouncementId }, announcement);
        }


    }
}
