using System.ComponentModel.DataAnnotations;

namespace Basket.API.Models;

public class ShoppingCartItem
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();

    public int Quantity { get; set; } = default!;
    public string Color { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string ShoppingCartUserName { get; set; } = default!;
}
