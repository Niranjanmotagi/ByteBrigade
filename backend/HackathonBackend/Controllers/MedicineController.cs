using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HackathonBackend.Data;
using HackathonBackend.Models;

namespace HackathonBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicineController(AppDbContext context)
        {
            _context = context;
        }

        // Anyone can browse medicines
        [HttpGet]
        public IActionResult GetAll()
        {
            var medicines = _context.Medicines.ToList();
            return Ok(medicines);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var medicine = _context.Medicines.Find(id);
            if (medicine == null) return NotFound();
            return Ok(medicine);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            _context.SaveChanges();
            return Ok(medicine);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, Medicine input)
        {
            var med = _context.Medicines.Find(id);
            if (med == null) return NotFound();

            med.Name = input.Name;
            med.Category = input.Category;
            med.Price = input.Price;
            med.StockQuantity = input.StockQuantity;
            med.Description = input.Description;
            med.RequiresPrescription = input.RequiresPrescription;
            med.ImageUrl = input.ImageUrl;

            _context.SaveChanges();
            return Ok(med);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var med = _context.Medicines.Find(id);
            if (med == null) return NotFound();

            _context.Medicines.Remove(med);
            _context.SaveChanges();
            return Ok(new { message = "Deleted" });
        }
    }
}
