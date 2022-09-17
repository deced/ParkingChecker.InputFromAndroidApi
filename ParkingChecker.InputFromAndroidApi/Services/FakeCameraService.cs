using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using ParkingChecker.InputFromAndrioidApi.Base.DataAccess;
using ParkingChecker.InputFromAndroidApi.Entities;

namespace ParkingChecker.InputFromAndroidApi.Services
{
    public class FakeCameraService : BackgroundService
    {
        private const string ImagesDirectory = "/home/deced/Desktop/ParkingChecker.ProcessingApi/dataset";
        private const int PauseInMilliseconds = 20000;
        
        
        
        
        private readonly IBaseRepository<ParkingImage> _parkingImageRepository;
        private readonly IBaseRepository<Parking> _parkingRepository;

        public FakeCameraService(IServiceProvider services)
        {
            using IServiceScope scope = services.CreateScope();
            _parkingRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<Parking>>();
            _parkingImageRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<ParkingImage>>();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("FakeCameraService is running.");
            
            var parkingId = ((long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
         
            await _parkingRepository.InsertOneAsync(new Parking()
            {
                Name = "Тестовая парковка " + DateTime.Now.Millisecond,
                ParkingId = parkingId
            });
           
            int i = 1;
            while (stoppingToken.IsCancellationRequested == false)
            {
                if (!File.Exists(Path.Combine(ImagesDirectory, $"Screenshot_{i}.png")))
                    i = 1;
                
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), $"{parkingId}.png");
                Console.WriteLine(fullPath);
                File.Copy(Path.Combine(ImagesDirectory, $"Screenshot_{i}.png"), fullPath,true);
                await InsertParkingImage(parkingId, fullPath);
                Thread.Sleep(PauseInMilliseconds);
                i++;
            }
        }

        private async Task InsertParkingImage(string parkingId, string fullPath)
        {
            var parkingImage = await _parkingImageRepository.FindOneAsync(x => x.ParkingId == parkingId);
            if (parkingImage == null)
            {
                await _parkingImageRepository.InsertOneAsync(new ParkingImage()
                {
                    FullPath = fullPath,
                    ParkingId = parkingId
                });
            }
            else
            {
                parkingImage.FullPath = fullPath;
                await _parkingImageRepository.ReplaceOneAsync(parkingImage);
            }
        }
    }
}