using BuildingBlocks.Messaging;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class StockNotReservedEventConsumerDefinition : ConsumerDefinition<StockNotReservedEventConsumer>
{
    public StockNotReservedEventConsumerDefinition()
    {
        EndpointName = RabbitMQSettings.Order_StockNotReservedEventQueue;
    }
}