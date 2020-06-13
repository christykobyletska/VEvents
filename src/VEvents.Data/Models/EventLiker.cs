using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VEvents.Data.Models
{
    public class EventLiker
    {
        private string _id;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get => _id ?? string.Empty; set => _id = value ?? string.Empty; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string EventId { get; set; }

        public string UserId { get; set; }
    }
}
