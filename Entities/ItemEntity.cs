using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTHerhaling.Entities
{
    class ItemEntity : TableEntity
    {
        public ItemEntity(string ItemSoldId, string location)
        {
            this.PartitionKey = ItemSoldId; // partition key komt van table entity
            this.RowKey = location;
        }
        public ItemEntity()
        {

        }
        
        public string DeviceId { get; set; }
        public string Item { get; set; }
        public string Empty { get; set; }
        public int ItemPrice { get; set; }
        public int Unix { get; set; }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Location { get; set; }
    }
}
