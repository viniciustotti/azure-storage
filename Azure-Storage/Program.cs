using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Azure_Storage.QueueStorage;
using Azure_Storage.TableStorage;
using Newtonsoft.Json;

namespace Azure_Storage
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            //LoadPizzaMenu(connectionString);
            //AddOrder(connectionString, Guid.Parse("")); // TODO Grab the Id from Table Storage
            //PrepareNextOrder(connectionString);


            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static void LoadPizzaMenu(string connectionString)
        {
            var pizzas = new List<PizzaEntity>
            {
                new PizzaEntity("Pepperoni", 10),
                new PizzaEntity("Meat Lovers", 11),
                new PizzaEntity("Canadian", 12),
                new PizzaEntity("Supreme Lovers", 13),
                new PizzaEntity("Veggie Lovers", 14),
                new PizzaEntity("Cheese Lovers", 15),
            };

            var pizzaRepository = new TableStorageRepository<PizzaEntity>(connectionString, PizzaEntity.TableName);

            foreach (var pizza in pizzas)
            {
                pizzaRepository.AddOrUpdateAsync(pizza).GetAwaiter().GetResult();
                Console.WriteLine($"Added pizza: {pizza.Name}");
            }
        }

        private static void AddOrder(string connectionString, Guid pizzaId)
        {
            var orderPizzaQueue = new OrderPizzaQueue(connectionString);

            var newOrder = new OrderPizzaMessage(pizzaId);

            orderPizzaQueue.AddMessageAsync(newOrder).GetAwaiter().GetResult();
            Console.WriteLine($"Order {newOrder.OrderId} added to queue {OrderPizzaQueue.QueueName}");
        }

        private static void PrepareNextOrder(string connectionString)
        {
            var orderPizzaQueue = new OrderPizzaQueue(connectionString);

            var messages = orderPizzaQueue.GetMessagesAsync(1, TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();

            if (messages.Any())
            {
                foreach (var message in messages)
                {
                    var orderPizzaMessage = JsonConvert.DeserializeObject<OrderPizzaMessage>(message.AsString);

                    Console.WriteLine($"Preparing order: {orderPizzaMessage.OrderId}");

                    // Preparing
                    Task.Delay(TimeSpan.FromSeconds(10));

                    Console.WriteLine($"Pizza {orderPizzaMessage.OrderId} is ready for pick up!");

                    orderPizzaQueue.DeleteMessageAsync(message).GetAwaiter().GetResult();
                }
            }
        }
    }
}
