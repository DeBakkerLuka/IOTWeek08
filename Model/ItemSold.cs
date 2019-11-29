using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTHerhaling.Model
{
    class ItemSold
    {
        [JsonProperty(PropertyName = "pcLuka")] // moet eigenlijk device_id zijn zoals in de python code.
        public string Device_Id { get; set; }
        public string Item { get; set; }
        public string Empty { get; set; }
        public int Price { get; set; }
        public int Unix { get; set; }
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Location { get; set; }
    }
}
