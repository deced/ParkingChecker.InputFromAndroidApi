using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ParkingChecker.InputFromAndroidApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InputController : ControllerBase
    {
        public string HealthCheck()
        {
            return "working";
        }
        
        [HttpPost]
        public async Task<IActionResult> SendImage()
        {
            StreamReader sr = new StreamReader(HttpContext.Request.Body);
            Console.WriteLine(await sr.ReadToEndAsync());
            return Ok();
        }
    }
}