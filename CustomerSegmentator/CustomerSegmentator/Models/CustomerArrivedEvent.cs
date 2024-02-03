using System.ComponentModel.DataAnnotations;

namespace CustomerSegmentator.Models;

public class CustomerArrivedEvent
{
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    [Range(1, 25, ErrorMessage = "Value must be between 1 and 25.")]
    public int Quantity { get; set; }
    public string? Segment { get; set; }
    public string? Tarriff { get; set; }
    public string? PaymentType { get; set; }
}
