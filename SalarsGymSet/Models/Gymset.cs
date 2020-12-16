using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalarsGymSet.Models
{
    public class Gymset
    {
        public ObjectId Id { get; set; }
        public string User { get; set; }
        public string Exercise { get; set; }
        public string Set { get; set; }
        public string Difficulty { get; set; }
        [DataType(DataType.Date)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }
    }
}
