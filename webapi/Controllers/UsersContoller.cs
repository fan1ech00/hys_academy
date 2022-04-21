using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppContext _db;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, AppContext db)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _db.Users.ToArrayAsync();
        return users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetAsync(long id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddAsync(User user)
    {
        // TODO: check fields of user or throw exception

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAsync), new { id = user.Id }, user);
    }

    [HttpPut]
    public async Task<ActionResult<User>> UpdateAsync(User user)
    {
        // TODO: check fields of user or throw exception

        if (await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id) is null)
            return NotFound();
            // return await Add(user);
        
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteAsync(long id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if (user is not null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
        return NoContent();
    }
}