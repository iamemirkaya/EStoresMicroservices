using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging
{
    public static class RabbitMQSettings
    {
        public const string Order_BasketCheckoutEventQueue = "order-basket-checkout-event-queue";
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";


        public const string Order_StockReservedEventQueue = "order-stock-reserved-event-queue";
        public const string Order_StockNotReservedEventQueue = "order-stock-not-reserved-event-queue";
    }
}
