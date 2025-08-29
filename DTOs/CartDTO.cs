namespace E_commerceAPI.DTOs
{
    public class CartDTO
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
