using System;
using Microsoft.Azure.Cosmos.Table;

namespace Azure_Storage.TableStorage
{
    public class PizzaEntity : TableEntity
    {
        public const string TableName = "PizzaEntity";

        public PizzaEntity()
        {
            PartitionKey = "TRex";
        }

        public PizzaEntity(string name, double price)
        {
            PartitionKey = "TRex";
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }

        private Guid _id;

        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                RowKey = _id.ToString();
            }
        }

        public string Name { get; set; }
        public double Price { get; set; }
    }
}
