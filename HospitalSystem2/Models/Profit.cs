using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystem2.Models
{
    public class Profit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow.AddHours(4);
        public string By { get; set; }
       public bool IsDeactive { get; set; }
    }
}
