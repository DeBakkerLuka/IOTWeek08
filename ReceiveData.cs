//using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Azure.EventHubs;
//using System.Text;
//using System.Net.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Table;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using IoTHerhaling.Model;
//using IoTHerhaling.Entities;
//using Microsoft.Azure.Devices;

//namespace IoTHerhaling
//{
//    public static class ReceiveData
//    {
//        private static HttpClient client = new HttpClient();

//        [FunctionName("ReceiveData")]
//        public static async Task Run([IoTHubTrigger("messages/events", Connection = "IOTHubConnectionString")]EventData message, ILogger log)
//        {
//            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");

//            try
//            {
//                string values = Encoding.UTF8.GetString(message.Body.Array);
//                string ConnectionString = Environment.GetEnvironmentVariable("AzureFunction");// pakt de ontvangen tekst 
//                //string json = await new StreamReader(values).ReadToEndAsync(); // leest alles in als json formaat
//                ItemSold newmessage = JsonConvert.DeserializeObject<ItemSold>(values);
//                string messageid = Guid.NewGuid().ToString();
//                newmessage.ItemSoldId = messageid;
//                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
//                CloudTableClient tableclient = storageAccount.CreateCloudTableClient();
//                CloudTable table = tableclient.GetTableReference("DrankAutomaat"); //case sensitive

//                ItemEntity ent = new ItemEntity(newmessage.ItemSoldId, newmessage.Location)
//                {
//                    Device_Id = newmessage.Device_Id,
//                    Item = newmessage.Item,
//                    Empty = newmessage.Empty,
//                    ItemPrice = newmessage.Price,

//                };
//                TableOperation insertOperation = TableOperation.Insert(ent);
//                await table.ExecuteAsync(insertOperation);
//                //return new OkObjectResult(newmessage);


//            }
//            catch (Exception ex)
//            {
//                log.LogError(ex, "ItemSold");
//                //return new OkObjectResult(500);
//            }
//        }
//    }
//}