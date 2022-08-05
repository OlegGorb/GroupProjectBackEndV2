using GroupProjectBackEndV2.Data;
using GroupProjectBackEndV2.Data.Models;
using GroupProjectBackEndV2.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GroupProjectBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly AppDbContext _context;
        public AuthHelper _helper;
        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _helper = new AuthHelper(configuration);
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            DateTime earlistTimeStamp = DateTime.UtcNow.AddMonths(-1);
            List<User> users = _context.Users
                .Include(u => u.Program)
                .Include(u => u.TimeSpends
                    .Where(t => t.StartDateTime >= earlistTimeStamp))
                .ToList();

            //foreach (var user in users)
            //{
            //    CorrectTime(user);
            //}

            return users;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Auth")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = _context.Users
                .Include(u => u.TimeSpends)
                .Include(u => u.Program)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            User userFromDb = await _context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

            if (userFromDb == null)
            {
                throw new Exception("User is not found");
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(userFromDb, userFromDb.Password, user.Password);

            if (result != PasswordVerificationResult.Success)
            {
                throw new Exception("Wrong credentials");
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("Admin", userFromDb.Admin.ToString()),
            };
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(30);

            TimeSpend session = new TimeSpend()
            {
                User = userFromDb,
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddMinutes(30),

            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            user.TimeSpends = null;
            session.User = null;

            return Ok(new
            {
                access_token = _helper.CreateToken(claims, expiresAt),
                user = userFromDb,
                current_session = session
            });
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize(Policy = "Auth")]
        public async Task<IActionResult> Logout([FromBody] int sessionId)
        {
            TimeSpend session = await _context.Sessions.FindAsync(sessionId);

            if (session == null)
            {
                throw new Exception("User is not found");
            }

            session.EndDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok();
        }

        //private void CorrectTime(User user)
        //{
        //    if (user.TimeSpends != null && user.TimeSpends.Count > 1)
        //    {
        //        List<TimeSpend> timeSpends = user.TimeSpends.ToList();
        //        timeSpends.Sort((x, y) => DateTime.Compare(x.StartDateTime, y.StartDateTime));

        //        for (int i = 0; i < timeSpends.Count - 1; i++)
        //        {
        //            if (timeSpends[i].EndDateTime > timeSpends[i + 1].StartDateTime)
        //            {
        //                timeSpends[i].EndDateTime = timeSpends[i + 1].StartDateTime;
        //            }
        //        }

        //        _context.SaveChanges();
        //    }
        //}

    }
}
