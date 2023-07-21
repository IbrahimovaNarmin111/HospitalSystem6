using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystem2.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get;set; }
        public string Description { get;set; }  
        public string Email { get;set; }
        public string Contact { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime RandevuDate { get; set; }
        public bool IsDeactive { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }

    }
}
