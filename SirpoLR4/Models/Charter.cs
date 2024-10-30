namespace SirpoLR4.Models
{
    public class Charter
    {
        public Guid Id { get; set; }
        public string? CititesPath { get; set; }
        public int Price { get; set; }
        public DateTime CharterDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
