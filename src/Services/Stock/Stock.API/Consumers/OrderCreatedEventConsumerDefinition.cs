using BuildingBlocks.Messaging;
using MassTransit;

namespace Stock.API.Consumers;

public class OrderCreatedEventConsumerDefinition : ConsumerDefinition<OrderCreatedEventConsumer>
{
    public OrderCreatedEventConsumerDefinition()
    {
        EndpointName = RabbitMQSettings.Stock_OrderCreatedEventQueue;
    }
}