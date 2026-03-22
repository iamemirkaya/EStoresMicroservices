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

public class StockReservedEventConsumer(IApplicationDbContext dbContext, ILogger<StockReservedEventConsumer> logger)
    : IConsumer<StockReservedEvent>
{
    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        logger.LogInformation("StockReservedEvent alındı. Sipariş durumu Completed yapılacak. OrderId: {OrderId}", context.Message.OrderId);

        var orderId = OrderId.Of(context.Message.OrderId);

        var order = await dbContext.Orders.FindAsync([orderId], context.CancellationToken);

        if (order != null)
        {
            order.ChangeStatus(OrderStatus.Completed);

            await dbContext.SaveChangesAsync(context.CancellationToken);
            logger.LogInformation("Sipariş başarıyla Completed olarak güncellendi. OrderId: {OrderId}", context.Message.OrderId);
        }
    }
}