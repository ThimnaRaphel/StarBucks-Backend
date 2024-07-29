namespace StarBucks_Backend.Models
{
    public class Sales
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
