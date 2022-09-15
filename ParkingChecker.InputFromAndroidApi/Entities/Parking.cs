using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using ParkingChecker.InputFromAndrioidApi.Base.DataAccess;

namespace ParkingChecker.InputFromAndroidApi.Entities
{
    [CollectionName("parking")]
    public class Parking : Document
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("parkingId")]
        public string ParkingId { get; set; }
    }
}