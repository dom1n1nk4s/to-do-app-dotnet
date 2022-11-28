using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using to_do_app_dotnet.DTOs.Entry;
using to_do_app_dotnet.Models;

namespace to_do_app_dotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EntryController : ControllerBase
    {
        private readonly IdentityContext _context;

        public EntryController(IdentityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntries()
        {
            return Ok(await _context.Entries.AsNoTracking().Where(x => x.UserName == User.Identity.Name).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntry(EntryDTO entryDTO)
        {
            var newEntry = new Entry { Title = entryDTO.Title, UserName = User.Identity.Name };
            await _context.Entries.AddAsync(newEntry);
            await _context.SaveChangesAsync();

            return Ok(newEntry);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(Guid id)
        {
            var entry = await _context.Entries.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name && x.Id == id);
            if (entry == null)
            {
                return BadRequest("No such entry found.");
            }
            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateEntry(EntryDTO entryDTO)
        {
            var entry = await _context.Entries.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name && x.Id == entryDTO.Id);
            if (entry == null)
            {
                return BadRequest("No such entry found.");
            }
            entry.Title = entryDTO.Title;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}