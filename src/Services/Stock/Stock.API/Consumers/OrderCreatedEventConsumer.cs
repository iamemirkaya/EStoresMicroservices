using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;

namespace Stock.API.Consumers;

public class OrderCreatedEventConsumer(StockDbContext dbContext, IPublishEndpoint publishEndpoint)
    : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        Console.WriteLine($"[Stock.API] OrderCreatedEvent alındı. Sipariş ID: {context.Message.OrderId}");

        var orderItems = context.Message.OrderItems;
        bool hasEnoughStock = true;

        foreach (var item in orderItems)
        {
            var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.ProductId == item.ProductId.ToString());
            if (stock == null || stock.Count < item.Quantity)
            {
                hasEnoughStock = false;
                break; 
            }
        }

        if (hasEnoughStock)
        {
            foreach (var item in orderItems)
            {
                var stock = await dbContext.Stocks.FirstAsync(s => s.ProductId == item.ProductId.ToString());
                stock.Count -= item.Quantity;
            }

            await dbContext.SaveChangesAsync();

            var stockReservedEvent = new StockReservedEvent
            {
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId,
                TotalPrice = context.Message.TotalPrice,
                OrderItems = orderItems
            };

            await publishEndpoint.Publish(stockReservedEvent);
            Console.WriteLine($"[Stock.API] Stocks updated successfully. StockReservedEvent published. Order ID: {context.Message.OrderId}");
        }
        else
        {
            var stockNotReservedEvent = new StockNotReservedEvent
            {
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId,
                Message = "Insufficient stock quantity..."
            };

            await publishEndpoint.Publish(stockNotReservedEvent);
            Console.WriteLine($"[Stock.API] INSUFFICIENT STOCK! StockNotReservedEvent published. Order ID: {context.Message.OrderId}");
        }
    }
}
