using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using ParkingChecker.InputFromAndrioidApi.Base.DataAccess;

namespace ParkingChecker.InputFromAndroidApi.Entities
{
    [CollectionName("parking_image")]
    public class ParkingImage : Document
    {
        [BsonElement("fullPath")]
        public string FullPath { get; set; }
        [BsonElement("parkingId")]
        public string ParkingId { get; set; }
    }
}