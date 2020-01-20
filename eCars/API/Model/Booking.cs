using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
