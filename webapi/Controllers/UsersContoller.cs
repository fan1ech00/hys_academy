using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Users.ToArrayAsync(ct);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetAsync(long id, CancellationToken ct)
    {
        var user = await _db.Users.FindAsync(new object[] { id }, ct);

        if (user is null)
            return NotFound();
        
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddAsync(User user, CancellationToken ct)
    {
        user.Id = 0;
        
        // GeneralValidateUser(user, ModelState);
        // if (!ModelState.IsValid)
        //     return BadRequest(ModelState);
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetAsync), new { id = user.Id }, user);
    }

    [HttpPut]
    public async Task<ActionResult<User>> UpdateAsync(User user, CancellationToken ct)
    {
        // if ID not specified or user not found in DB
        if (user.Id == 0 || await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id, ct) is null)
            return await AddAsync(user, ct);
        
        // GeneralValidateUser(user, ModelState);
        // if (!ModelState.IsValid)
        //     return BadRequest(ModelState);
        
        _db.Users.Update(user);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteAsync(long id, CancellationToken ct)
    {
        var user = await _db.Users.FindAsync(new object[] { id }, ct);

        if (user is null) 
            return NoContent();
        
        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [NonAction]
    private static void GeneralValidateUser(User user, ModelStateDictionary modelState)
    {
        if (string.IsNullOrEmpty(user.Name))
            modelState.AddModelError(nameof(user.Name), "Name is empty");
        
        if (user.Age <= 0 || user.Age >= 100)
            modelState.AddModelError(nameof(user.Age), "Age must be > 0 and < 100");
    }
}