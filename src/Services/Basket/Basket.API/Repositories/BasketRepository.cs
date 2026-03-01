using Basket.API.Models;
using Basket.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data;

public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        return basket ?? throw new Exception($"Basket not found for user: {userName}");
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.ShoppingCarts.AnyAsync(x => x.UserName == basket.UserName, cancellationToken);

        if (exists)
        {
            dbContext.ShoppingCarts.Update(basket);
        }
        else
        {
            dbContext.ShoppingCarts.Add(basket);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        var deletedCount = await dbContext.ShoppingCarts
            .Where(x => x.UserName == userName)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0;
    }
}