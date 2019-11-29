using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Azure.Cosmos;
using IoTHerhaling.Model;
using Newtonsoft.Json;
using IoTHerhaling.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;

namespace IoTHerhaling
{
    public static class CosmosDB
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("CosmosDB")]
        public static async System.Threading.Tasks.Task RunAsync([IoTHubTrigger("messages/events", Connection = "IOTHubConnectionString")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            try
            {
                CosmosClientOptions clientOPtions = new CosmosClientOptions();
                clientOPtions.ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway;
                CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDBConnectionString"), clientOPtions);
                Container container = cosmosClient.GetContainer("week7", "Container1");
        
                //ContainerProperties containerProperties = await container.ReadContainerAsync();


                string values = Encoding.UTF8.GetString(message.Body.Array);
                //string ConnectionString = Environment.GetEnvironmentVariable("AzureFunction");// pakt de ontvangen tekst 
                //string json = await new StreamReader(values).ReadToEndAsync(); // leest alles in als json formaat
                ItemSold newmessage = JsonConvert.DeserializeObject<ItemSold>(values);
                newmessage.Id = Guid.NewGuid();
                newmessage.Unix = (int)(DateTime.UtcNow.Date.Subtract(new DateTime(1970,1,1,0,0,0))).TotalDays;
                await container.CreateItemAsync(newmessage, new PartitionKey(newmessage.Device_Id));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}