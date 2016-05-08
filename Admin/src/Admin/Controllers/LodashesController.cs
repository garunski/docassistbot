using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Admin.Models;
using Microsoft.AspNet.Authorization;

namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/Lodashes")]
    public class LodashesController : Controller
    {
        private ApplicationDbContext _context;

        public LodashesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lodashes
        [HttpGet]
        public IEnumerable<Lodash> GetLodash()
        {
            return _context.Lodash;
        }

        // GET: api/Lodashes/5
        [HttpGet("{id}", Name = "GetLodash")]
        public async Task<IActionResult> GetLodash([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            Lodash lodash = await _context.Lodash.SingleAsync(m => m.Id == id);

            if (lodash == null)
            {
                return HttpNotFound();
            }

            return Ok(lodash);
        }

        // PUT: api/Lodashes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLodash([FromRoute] int id, [FromBody] Lodash lodash)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            if (id != lodash.Id)
            {
                return HttpBadRequest();
            }

            _context.Entry(lodash).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LodashExists(id))
                {
                    return HttpNotFound();
                }
                else
                {
                    throw;
                }
            }

            return new HttpStatusCodeResult(StatusCodes.Status204NoContent);
        }

        // POST: api/Lodashes
        [HttpPost]
        public async Task<IActionResult> PostLodash([FromBody] Lodash lodash)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            _context.Lodash.Add(lodash);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LodashExists(lodash.Id))
                {
                    return new HttpStatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetLodash", new { id = lodash.Id }, lodash);
        }

        // DELETE: api/Lodashes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLodash([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            Lodash lodash = await _context.Lodash.SingleAsync(m => m.Id == id);
            if (lodash == null)
            {
                return HttpNotFound();
            }

            _context.Lodash.Remove(lodash);
            await _context.SaveChangesAsync();

            return Ok(lodash);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LodashExists(int id)
        {
            return _context.Lodash.Count(e => e.Id == id) > 0;
        }
    }
}