using VEvents.Data.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VEvents.Data.Models
{
    public class Event
    {
        private string _id;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get => _id ?? string.Empty; set => _id = value ?? string.Empty; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime DateAndTime { get; set; }
        public EventStatus Status { get; set; }
        public string PublisherId { get; set; }
    }
}
