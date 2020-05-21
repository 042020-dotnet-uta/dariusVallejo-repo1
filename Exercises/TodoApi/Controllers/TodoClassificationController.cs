using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoClassificationController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoClassificationController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoClassification>>> GetTodoClassification()
        {
            return await _context.TodoClassification.ToListAsync();
        }

        // GET: api/TodoClassification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoClassification>> GetTodoClassification(long id)
        {
            var todoClassification = await _context.TodoClassification.FindAsync(id);

            if (todoClassification == null)
            {
                return NotFound();
            }

            return todoClassification;
        }

        // PUT: api/TodoClassification/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoClassification(long id, TodoClassification todoClassification)
        {
            if (id != todoClassification.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoClassification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoClassificationExists(id))
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

        // POST: api/TodoClassification
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoClassification>> PostTodoClassification(TodoClassification todoClassification)
        {
            _context.TodoClassification.Add(todoClassification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoClassification", new { id = todoClassification.Id }, todoClassification);
        }

        // DELETE: api/TodoClassification/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoClassification>> DeleteTodoClassification(long id)
        {
            var todoClassification = await _context.TodoClassification.FindAsync(id);
            if (todoClassification == null)
            {
                return NotFound();
            }

            _context.TodoClassification.Remove(todoClassification);
            await _context.SaveChangesAsync();

            return todoClassification;
        }

        private bool TodoClassificationExists(long id)
        {
            return _context.TodoClassification.Any(e => e.Id == id);
        }
    }
}
