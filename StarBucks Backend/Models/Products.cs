namespace StarBucks_Backend.Models
{
    public class Products
    {
            public int ID { get; set; }
            public string Category { get; set; }
            public string ProductName { get; set; }
            public string Details { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
    }
}
