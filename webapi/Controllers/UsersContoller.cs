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
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await _db.Users.ToArrayAsync();

        if (users.Length == 0)
            return NotFound();
        
        return users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(long id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(User user)
    {
        _db.Users.Add(user);
        // TODO: check fields of user or throw exception
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Put(long id, User user)
    {
        if (id != user.Id)
            return BadRequest();
        
        if (await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) is null)
            return NotFound();
        
        // TODO: check fields of user or throw exception

        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> Delete(long id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if (user is null)
            return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}