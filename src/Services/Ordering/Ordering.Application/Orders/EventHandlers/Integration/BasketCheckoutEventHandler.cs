using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Integration;
public class BasketCheckoutEventHandler
    (ISender sender, ILogger<BasketCheckoutEventHandler> logger, IPublishEndpoint publishEndpoint) // publishEndpoint EKLENDİ
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var command = MapToCreateOrderCommand(context.Message);

        var result = await sender.Send(command);

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = command.Order.Id, 
            CustomerId = context.Message.CustomerId,
            TotalPrice = context.Message.TotalPrice,
            OrderItems = context.Message.Items 
        };

        logger.LogInformation("Publishing OrderCreatedEvent for OrderId: {OrderId}", orderCreatedEvent.OrderId);

        await publishEndpoint.Publish(orderCreatedEvent);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
        var orderId = Guid.NewGuid();

        var orderItems = message.Items.Select(item =>
            new OrderItemDto(orderId, item.ProductId, item.Quantity, item.Price)
        ).ToList();

        var orderDto = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: Ordering.Domain.Enums.OrderStatus.Pending,
            OrderItems: orderItems
        );

        return new CreateOrderCommand(orderDto);
    }
}