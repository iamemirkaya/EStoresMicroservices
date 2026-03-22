using BuildingBlocks.Messaging;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Integration;
public class BasketCheckoutEventConsumerDefinition : ConsumerDefinition<BasketCheckoutEventHandler>
{
    public BasketCheckoutEventConsumerDefinition()
    {
        EndpointName = RabbitMQSettings.Order_BasketCheckoutEventQueue;
    }
}
