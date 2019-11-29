using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using IoTHerhaling.Model;
using System.Collections.Generic;

namespace IoTHerhaling
{
    public static class GetMoneyGainedByDeviceLocation
    {
        [FunctionName("GetMoneyGainedByDeviceLocation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "money/{location}/{device}")] HttpRequest req, string location, string device,
            ILogger log)
        {
            try
            {
                CosmosClientOptions clientOPtions = new CosmosClientOptions();
                clientOPtions.ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway;
                CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDBConnectionString"), clientOPtions);
                Container container = cosmosClient.GetContainer("week7", "Container1");
                QueryDefinition query = new QueryDefinition(
                   "select * from Documents s where s.Location = @location AND s.pcLuka = null")
                 .WithParameter("@location", location);
                
                FeedIterator<ItemSold> resultSet = container.GetItemQueryIterator<ItemSold>(query);
                int teller = 0;

                List<ItemSold> Items = new List<ItemSold>();
                while (resultSet.HasMoreResults)
                {

                    FeedResponse<ItemSold> response = await resultSet.ReadNextAsync();
                    foreach (ItemSold item in response)
                    {
                        Items.Add(item);
                        teller += item.Price;
                    }

                }
                return new OkObjectResult(teller);
            }
            catch (Exception ex)
            {

                throw ex;
                return new StatusCodeResult(200);
            }
        }
    }
}
