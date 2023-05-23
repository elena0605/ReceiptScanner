namespace ReceiptScanner
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public bool Domestic { get; set; }
        public decimal Price { get; set; }
        public int? Weight { get; set; }
        public string Description { get; set; } = string.Empty; 
    }
}
