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
    public class ModelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Models
        [HttpGet]
        public async Task<ActionResult> GetModels()
        {
            var result = await _context.Models.Include(x => x.Brand).Select(x => new ModelDto{ 
                Id = x.Id,
                Name = x.Name,
                BrandId = x.BrandId,
                BrandName = x.Brand.Name
            }).ToListAsync();
            return Ok(result);
        }

        // GET: api/Models/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelDto>> GetModel(int id)
        {
            var model = await _context.Models.Include(x => x.Brand).Select(x => new ModelDto
            {
                Id = x.Id,
                Name = x.Name,
                BrandId = x.BrandId,
                BrandName = x.Brand.Name
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        // PUT: api/Models/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(int id, Model model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
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

        // POST: api/Models
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel(Model model)
        {
            _context.Models.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModel", new { id = model.Id }, model);
        }

        // DELETE: api/Models/5
        //[Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Model>> DeleteModel(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        private bool ModelExists(int id)
        {
            return _context.Models.Any(e => e.Id == id);
        }
    }
}
