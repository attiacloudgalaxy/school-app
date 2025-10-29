using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly SchoolContext _context;
    public ClassesController(SchoolContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Classroom>>> Get()
    {
        var classes = await _context.Classrooms
            .Include(c => c.Students)
            .ToListAsync();
        return Ok(classes);
    }
}
