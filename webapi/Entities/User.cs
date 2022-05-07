using System.ComponentModel.DataAnnotations;

namespace webapi.Entities;

public class User
{
    public long Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Range(1, 100)]
    public int Age { get; set; }
}