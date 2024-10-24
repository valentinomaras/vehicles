using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleManagement.Data;
using VehicleManagement.Models;

namespace VehicleManagement.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly VehicleManagementContext _context;

        public ModelsController(VehicleManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all models, optionally filtered by brand ID.
        /// </summary>
        /// <param name="brandId">The ID of the brand to filter models (optional).</param>
        /// <returns>A list of models.</returns>
        [HttpGet]
        public IActionResult GetModels(int? brandId = null) // Allow for optional brandId
        {
            var models = brandId.HasValue
                ? _context.Models.Where(m => m.BrandID == brandId.Value).Include(m => m.Brand).ToList()
                : _context.Models.Include(m => m.Brand).ToList();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public IActionResult GetModel(int id)
        {
            var model = _context.Models.Include(m => m.Brand).FirstOrDefault(m => m.ModelID == id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public IActionResult PostModel([FromBody] VehicleModel model)
        {
            _context.Models.Add(model);
            _context.SaveChanges();
            return CreatedAtAction("GetModel", new { id = model.ModelID }, model);
        }


        [HttpPut("{id}")]
        public IActionResult PutModel(int id, [FromBody] VehicleModel model)
        {
            if (id != model.ModelID)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteModel(int id)
        {
            var model = _context.Models.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
