using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParkingChecker.InputFromAndrioidApi.Base.DataAccess
{
    public abstract class Document : IDocument
    {
        [BsonId]  
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }
        

        protected Document()
        {
            DateTime now = DateTime.UtcNow;
            CreationDate = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
        }
    }
}