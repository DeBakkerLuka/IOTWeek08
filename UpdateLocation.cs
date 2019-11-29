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

namespace IoTHerhaling
{
    public static class UpdateLocation
    {
        [FunctionName("UpdateLocation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "updateLocation/{deviceId}={Location}")] HttpRequest req, string deviceId, string location,
            ILogger log)
        {
            RegistryManager manager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("AdminIoTHubConnectionString"));
            var twin = await manager.GetTwinAsync(deviceId);
            twin.Properties.Desired["location"] = location;
            await manager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
            return new StatusCodeResult(200);
        }
    }
}
