using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleManagement.Data;
using VehicleManagement.Models;

namespace VehicleManagement.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleManagementContext _context;

        public VehiclesController(VehicleManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetVehicles()
        {
            var vehicles = _context.Vehicles.Include(v => v.Brand).Include(v => v.Model).ToList();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _context.Vehicles.Include(v => v.Brand).Include(v => v.Model).FirstOrDefault(v => v.VehicleID == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPost]
        public IActionResult PostVehicle([FromBody] Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return CreatedAtAction("GetVehicle", new { id = vehicle.VehicleID }, vehicle);
        }

        [HttpPut("{id}")]
        public IActionResult PutVehicle(int id, [FromBody] Vehicle vehicle)
        {
            if (id != vehicle.VehicleID)
            {
                return BadRequest("Vehicle ID mismatch.");
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Vehicles.Any(v => v.VehicleID == id))
                {
                    return NotFound("Vehicle not found.");
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
