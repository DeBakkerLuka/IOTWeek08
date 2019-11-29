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
    public static class GetAllItemsSoldLocation
    {
        [FunctionName("GetAllItemsSoldLocation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "items/{location}")] HttpRequest req, string location,
            ILogger log)
        {
            try
            {
                CosmosClientOptions clientOPtions = new CosmosClientOptions();
                clientOPtions.ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway;
                CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDBConnectionString"), clientOPtions);
                Container container = cosmosClient.GetContainer("week7", "Container1");
                QueryDefinition query = new QueryDefinition(
                   "select * from Documents s where s.Location = @location")
                 .WithParameter("@location", location);
                FeedIterator<ItemSold> resultSet = container.GetItemQueryIterator<ItemSold>(query);


                List<ItemSold> Items = new List<ItemSold>();
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<ItemSold> response = await resultSet.ReadNextAsync();
                    foreach (ItemSold item in response)
                    {
                        Items.Add(item);

                    }

                }
                return new OkObjectResult(Items);
            }
            catch (Exception ex)
            {

                throw ex;
                return new StatusCodeResult(200);
            }
        }
    }
}