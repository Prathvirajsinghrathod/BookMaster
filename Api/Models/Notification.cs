using System.ComponentModel.DataAnnotations.Schema;

namespace BookMaster.Api.Models;

public class Notification
{
    public long Id { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }
    public User? User { get; set; }

    [Column("is_read")]
    public bool IsRead { get; set; } = false;
}
