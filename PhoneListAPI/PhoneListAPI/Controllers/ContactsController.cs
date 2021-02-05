using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneListAPI.Data;
using PhoneListAPI.Models;

namespace PhoneListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly PhoneListContext _context;

        public ContactsController(PhoneListContext context)
        {
            _context = context;
        }

        // GET: api/PhoneLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneList>>> GetPhoneList()
        {
            return await _context.PhoneList.ToListAsync();
        }

        // GET: api/PhoneLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneList>> GetPhoneList(int id)
        {
            var phoneList = await _context.PhoneList.FindAsync(id);

            if (phoneList == null)
            {
                return NotFound();
            }

            return phoneList;
        }

        // GET: api/findContactByName?name={}
        [HttpGet("findContactByName")]
        public async Task<ActionResult<PhoneList>> Get([FromQuery] string name)
        {
            var contacts = await _context.PhoneList.Where(p => p.Name.Contains(name)).ToListAsync();

            if(contacts == null)
            {
                return NotFound();
            }

            return Ok(contacts);
        }
        
        // PUT: api/PhoneLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneList(int id, PhoneList phoneList)
        {
            if (id != phoneList.Id)
            {
                return BadRequest();
            }

            _context.Entry(phoneList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneListExists(id))
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

        // POST: api/PhoneLists
        [HttpPost]
        public async Task<ActionResult<PhoneList>> PostPhoneList(PhoneList phoneList)
        {
            _context.PhoneList.Add(phoneList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhoneList", new { id = phoneList.Id }, phoneList);
        }

        // DELETE: api/PhoneLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhoneList>> DeletePhoneList(int id)
        {
            var phoneList = await _context.PhoneList.FindAsync(id);
            if (phoneList == null)
            {
                return NotFound();
            }

            _context.PhoneList.Remove(phoneList);
            await _context.SaveChangesAsync();

            return phoneList;
        }

        private bool PhoneListExists(int id)
        {
            return _context.PhoneList.Any(e => e.Id == id);
        }
    }
}
