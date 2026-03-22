namespace Stock.API.Models
{
    public class Stock
    {
        public Guid Id { get; set; }

        public string ProductId { get; set; }

        public int Count { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
