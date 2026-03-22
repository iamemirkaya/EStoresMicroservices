using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class StockNotReservedEventConsumer(IApplicationDbContext dbContext, ILogger<StockNotReservedEventConsumer> logger)
    : IConsumer<StockNotReservedEvent>
{
    public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
    {
        logger.LogWarning("StockNotReservedEvent alındı! Stok yetersiz. Sipariş iptal ediliyor. OrderId: {OrderId}, Sebep: {Message}", context.Message.OrderId, context.Message.Message);

        var orderId = OrderId.Of(context.Message.OrderId);
        var order = await dbContext.Orders.FindAsync([orderId], context.CancellationToken);

        if (order != null)
        {
            order.ChangeStatus(OrderStatus.Cancelled);

            await dbContext.SaveChangesAsync(context.CancellationToken);
            logger.LogInformation("Sipariş stok yetersizliğinden dolayı Cancelled olarak güncellendi. OrderId: {OrderId}", context.Message.OrderId);
        }
    }
}