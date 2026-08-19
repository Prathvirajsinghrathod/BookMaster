using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMaster.Api.Models;

public static class ExchangeRequestStatus
{
    public const string Pending = "PENDING";
    public const string Accepted = "ACCEPTED";
    public const string Rejected = "REJECTED";
}

public class ExchangeRequest
{
    public long Id { get; set; }

    [Column("listing_id")]
    public long ListingId { get; set; }
    public ExchangeListing? Listing { get; set; }

    [Column("requester_id")]
    public long RequesterId { get; set; }
    public User? Requester { get; set; }

    [Column("offered_book_id")]
    public long OfferedBookId { get; set; }
    public Book? OfferedBook { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = ExchangeRequestStatus.Pending;

    public History? History { get; set; }
}
