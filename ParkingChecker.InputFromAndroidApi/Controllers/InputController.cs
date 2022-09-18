using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using ParkingChecker.InputFromAndrioidApi.Base.DataAccess;
using ParkingChecker.InputFromAndroidApi.Entities;
using ParkingChecker.InputFromAndroidApi.Models;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ParkingChecker.InputFromAndroidApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InputController : Controller
    {
        private readonly IBaseRepository<Parking> _parkingRepository;
        private readonly IBaseRepository<ParkingImage> _parkingImageRepository;

        public InputController(IBaseRepository<Parking> parkingRepository,
            IBaseRepository<ParkingImage> parkingImageRepository)
        {
            _parkingRepository = parkingRepository;
            _parkingImageRepository = parkingImageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public string HealthCheck()
        {
            return "working";
        }

        [HttpPost]
        public async Task<IActionResult> RegisterParking([FromBody] string data)
        {
            var parkingModel = JsonConvert.DeserializeObject<RegisterParkingModel>(data);
            await _parkingRepository.InsertOneAsync(new Parking()
            {
                ParkingId = parkingModel.time,
                Name = parkingModel.name
            });
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendImage([FromBody] string data)
        {
            var parkingImageModel = JsonConvert.DeserializeObject<ParkingImageModel>(data);
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), parkingImageModel.time + ".png");
            Console.WriteLine(fullPath);
            System.IO.File.WriteAllBytes(parkingImageModel.time + ".png",
                Convert.FromBase64String(parkingImageModel.image));
            var parkingImage = await _parkingImageRepository.FindOneAsync(x => x.ParkingId == parkingImageModel.time);
            if (parkingImage == null)
            {
                await _parkingImageRepository.InsertOneAsync(new ParkingImage()
                {
                    FullPath = fullPath,
                    ParkingId = parkingImageModel.time
                });
            }
            else
            {
                parkingImage.FullPath = fullPath;
                await _parkingImageRepository.ReplaceOneAsync(parkingImage);
            }

            return Ok();
        }
    }
}