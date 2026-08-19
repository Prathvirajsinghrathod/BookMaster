using System.ComponentModel.DataAnnotations;

namespace BookMaster.Api.Models;

public class Category
{
    public long Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
