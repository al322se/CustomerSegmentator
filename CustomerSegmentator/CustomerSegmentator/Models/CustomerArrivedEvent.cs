namespace CustomerSegmentator.Models;

public class CustomerArrivedEvent
{
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public int Quantity { get; set; }
    public string? Segment { get; set; }
    public string? Tarriff { get; set; }
    public bool IsCash { get; set; }
}
