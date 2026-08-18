using System.ComponentModel.DataAnnotations;

namespace BookMaster.Api.Models;

public class User
{
    public long Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<ExchangeRequest> ExchangeRequests { get; set; } = new List<ExchangeRequest>();
}
