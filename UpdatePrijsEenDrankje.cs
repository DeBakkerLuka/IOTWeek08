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
    public static class UpdatePrijsEenDrankje
    {
        [FunctionName("UpdatePrijsEenDrankje")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "updatePriceDrink")] HttpRequest req,
            ILogger log)
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("AdminIoTHubConnectionString"));
            string json = await new StreamReader(req.Body).ReadToEndAsync();
            var commandMessage = new Message(Encoding.ASCII.GetBytes(json));
            await serviceClient.SendAsync("pcLuka",commandMessage);
            return new StatusCodeResult(200);

        }
    }
}
