using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Separate.Data;
using Separate.Data.Entities;
using Separate.Data.Enums;
using Separate.Models;

namespace Separate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult> GetCars()
        {
            var result = await _context.Cars.Include(x => x.Model).ThenInclude(x => x.Brand).Select(MapCar()).ToListAsync();
            return Ok(result);
        }

        private static System.Linq.Expressions.Expression<Func<Car, CarDto>> MapCar()
        {
            return x => new CarDto
            {
                Id = x.Id,
                ModelId = x.ModelId,
                ReleaseYear = x.ReleaseYear,
                Price = x.Price,
                Description = x.Description,
                ModelName = x.Model.Name,
                BrandName = x.Model.Brand.Name
            };
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            var car = await _context.Cars.Include(x => x.Model).ThenInclude(x => x.Brand).Select(MapCar()).FirstOrDefaultAsync(x => x.Id == id);
            if (car == null) return NotFound();
            return car;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = AppRoles.Client)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = AppRoles.Client)]
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        //[Authorize(Roles = AppRoles.Client)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
