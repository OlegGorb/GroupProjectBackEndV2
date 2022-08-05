using GroupProjectBackEndV2.Data;
using GroupProjectBackEndV2.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProjectBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        public readonly AppDbContext _context;
        public ProgramController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
        }

        [HttpPost]
        [Route("AddProgram")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<StudentProgram>> AddProgram([FromBody] StudentProgram program)
        {
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        [HttpDelete]
        [Route("DeleteProgram")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<StudentProgram>> DeleteProgram([FromBody] int id)
        {
            StudentProgram sp = _context.Programs.Find(id);

            if (sp == null)
            {
                throw new Exception("Couldn't find the program");
            }

            _context.Programs.Remove(sp);
            await _context.SaveChangesAsync();

            return sp;
        }

        [HttpGet]
        [Route("GetPrograms")]
        //[Authorize(Policy = "Auth")]
        public async Task<ActionResult<List<StudentProgram>>> GetPrograms()
        {
            List<StudentProgram> sps = _context.Programs.ToList();
            return sps;
        }

        [HttpPut]
        [Route("EditProgram")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<StudentProgram>> EditProgram([FromBody] StudentProgram program)
        {
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
            return program;
        }

        [HttpGet]
        [Route("GetProgram")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<StudentProgram>> GetProgram([FromBody] int id)
        {
            StudentProgram sp = _context.Programs.Find(id);

            if (sp == null)
            {
                throw new Exception("Couldn't find the program");
            }

            return sp;
        }

        [HttpPost]
        [Route("SignUp")]
        //[Authorize(Policy = "Auth")]
        public async Task<ActionResult> SignUp([FromBody] UserCourse userCourse)
        {
            User user = await _context.Users.FindAsync(userCourse.UserId);
            StudentProgram program = await _context.Programs.FindAsync(userCourse.CourseId);

            if (user != null && program != null)
            {
                user.Program = program;
            }

            _context.SaveChanges();

            return Ok();
        }

    }
}
