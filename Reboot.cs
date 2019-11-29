using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Rest;
using Microsoft.Azure.Devices;

namespace IoTHerhaling
{
    public static class Reboot
    {
        [FunctionName("Reboot")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "reboot/{deviceId}")] HttpRequest req, string deviceId,
            ILogger log)
        {
            try
            {
                ServiceClient serviceClient;
                string connectionString = Environment.GetEnvironmentVariable("AdminIoTHubConnectionString");
                serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
                CloudToDeviceMethod method = new CloudToDeviceMethod("reboot");
                method.SetPayloadJson("{'seconds':15}");
                await serviceClient.InvokeDeviceMethodAsync(deviceId, method);
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(200);

            }
        }
    }
}
