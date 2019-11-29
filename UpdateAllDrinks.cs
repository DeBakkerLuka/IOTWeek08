using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using System.Text;

namespace IoTHerhaling
{
    public static class UpdateAllDrinks
    {
        [FunctionName("UpdateAllDrinks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UpdateDrink/{drink}={price}")] HttpRequest req, string drink, int price,
            ILogger log)
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("AdminIoTHubConnectionString"));
            var commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            await serviceClient.SendAsync("pcLuka", commandMessage);
            return new StatusCodeResult(200);
        }
    }
}
