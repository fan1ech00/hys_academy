using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppContext _db;

    public UsersController(AppContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _db.Users.ToArrayAsync();;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetAsync(long id)
    {
        // find or FirstOrDefault? find быстрее
        var user = await _db.Users.FindAsync(id);

        if (user is null)
            return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddAsync(User user)
    {
        // как игнорировать входящий Id?
        user.Id = 0;
        
        // ModelState?
        if (string.IsNullOrEmpty(user.Name))
            return BadRequest();
        
        if (user.Age < 0 || user.Age > 100)
            return BadRequest();
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetAsync), new { id = user.Id }, user);
    }

    [HttpPut]
    public async Task<ActionResult<User>> UpdateAsync(User user)
    {
        if (user.Id == 0)
            return BadRequest();
        
        if (string.IsNullOrEmpty(user.Name))
            return BadRequest();
        
        if (user.Age < 0 || user.Age > 100)
            return BadRequest();
        
        
        if (await _db.Users.FindAsync(user.Id) is null)
            return NotFound();
            // return await Add(user);
        
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteAsync(long id)
    {
        var user = await _db.Users.FindAsync(id);
        
        if (user is not null)
        {
            _db.Users.Remove(user);
            // удалить await?
            await _db.SaveChangesAsync();
        }
        return NoContent();
    }
}