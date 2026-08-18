using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMaster.Api.Models;

public class ExchangeListing
{
    public long Id { get; set; }

    [Column("book_id")]
    public long BookId { get; set; }
    public Book? Book { get; set; }

    [Required, MaxLength(100)]
    [Column("wanted_type")]
    public string WantedType { get; set; } = string.Empty;

    public ICollection<ExchangeRequest> ExchangeRequests { get; set; } = new List<ExchangeRequest>();
}
