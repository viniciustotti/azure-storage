using System;

namespace Azure_Storage.QueueStorage
{
    public class OrderPizzaMessage
    {
        public Guid OrderId { get; set; }
        public Guid PizzaId { get; set; }
        public DateTime OrderDate { get; set; }

        public OrderPizzaMessage()
        {

        }

        public OrderPizzaMessage(Guid pizzaId)
        {
            PizzaId = pizzaId;
            OrderId = Guid.NewGuid();
            OrderDate = DateTime.UtcNow;
        }
    }
}
