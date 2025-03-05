namespace Consumer.Entities;

public class Order
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

