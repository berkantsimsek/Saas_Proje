namespace SaaSProje.Models
{
    public class Product : IMustHaveTenant
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int FirmId { get; set; } // Bu ürünün hangi firmaya ait olduğunu tutacak
    }
}