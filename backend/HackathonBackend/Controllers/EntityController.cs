using Microsoft.AspNetCore.Mvc;
using HackathonBackend.Models;
using HackathonBackend.Data;

namespace HackathonBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EntityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _context.Entities.ToList();

            return Ok(entities);
        }

        [HttpPost]
        public IActionResult Add(Entity entity)
        {
            _context.Entities.Add(entity);

            _context.SaveChanges();

            return Ok(entity);
        }
    }
}