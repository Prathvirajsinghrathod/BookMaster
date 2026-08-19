using System.ComponentModel.DataAnnotations.Schema;

namespace BookMaster.Api.Models;

public class History
{
    public long Id { get; set; }

    [Column("request_id")]
    public long RequestId { get; set; }
    public ExchangeRequest? Request { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }
}
