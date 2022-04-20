using Microsoft.EntityFrameworkCore;
using webapi.Entities;
using AppContext = webapi.AppContext;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
// builder.Services.AddDbContext<AppContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<AppContext>(options => options.UseInMemoryDatabase("hysdb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // add default data to database
    app.MapGet("/api/default", async (AppContext db, HttpContext context) =>
    {
        db.Users.Add(new User {Name = "User1", Age = 16});
        db.Users.Add(new User {Name = "User2", Age = 23});
        db.Users.Add(new User {Name = "User3", Age = 56});
        await db.SaveChangesAsync();

        context.Response.StatusCode = 200;
    });
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();



app.Run();