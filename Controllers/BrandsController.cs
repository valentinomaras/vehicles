using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleManagement.Data;
using VehicleManagement.Models;

namespace VehicleManagement.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly VehicleManagementContext _context;

        public BrandsController(VehicleManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetBrands()
        {
            var brands = _context.Brands.ToList();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public IActionResult GetBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpPost]
        public IActionResult PostBrand([FromBody] Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return CreatedAtAction("GetBrand", new { id = brand.BrandID }, brand);
        }

        [HttpPut("{id}")]
        public IActionResult PutBrand(int id, [FromBody] Brand brand)
        {
            if (id != brand.BrandID)
            {
                return BadRequest("Brand ID mismatch.");
            }

            // Validate the brand nam
            if (string.IsNullOrWhiteSpace(brand.BrandName))
            {
                return BadRequest("Brand name is required.");
            }

            // Set the entity state to modified
            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the brand exists in case of concurrency issues
                if (!BrandExists(id))
                {
                    return NotFound("Brand not found.");
                }
                throw; 
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        // Method to check if a brand exists
        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.BrandID == id); // Check existence of brand
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
