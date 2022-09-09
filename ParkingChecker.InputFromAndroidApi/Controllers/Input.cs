using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ParkingChecker.InputFromAndroidApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Input : ControllerBase
    {
        public string HealthCheck()
        {
            return "working";
        }
        
        [HttpPost]
        public IActionResult SendImage()
        {
            return Ok();
        }
    }
}