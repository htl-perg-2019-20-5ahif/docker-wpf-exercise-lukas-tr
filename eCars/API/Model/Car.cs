using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Model
{
    public class Car
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; }

        [JsonPropertyName("licensePlate")]
        public string LicensePlate { get; set; }

        public List<Booking> Bookings { get; set; }

        public string DisplayName => $"{Make} - {LicensePlate}";
    }
}
