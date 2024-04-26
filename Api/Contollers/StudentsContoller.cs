using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students.AsNoTracking().ToListAsync();

            return students;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.AddAsync(student);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if(student is null)
                return NotFound();

            return Ok(student);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student is null)
                return NotFound();
            _context.Students.Remove(student);

            var result = await _context.SaveChangesAsync();
            if(result > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        // api/students/1
        public async Task<IActionResult> EditStudent(int id, Student student)
        {
            var studentFromDb = await _context.Students.FindAsync(id);

            if(studentFromDb is null)
            {
                return BadRequest("Student not found");
            }

            studentFromDb.Name = student.Name;
            studentFromDb.Adress = student.Adress;
            studentFromDb.Email = student.Email;
            studentFromDb.PhoneNumber = student.PhoneNumber;

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
