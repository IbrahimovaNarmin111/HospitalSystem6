using HospitalSystem2.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HospitalSystem2.Models
{
    public class Doctor
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Email { get;set; }
        public string Contact { get; set; }
        public decimal Salary { get; set; }
        public bool IsDeactive { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<ReceptionEnumModel> Receptions { get; set; }
        
        [NotMapped]
        public IFormFile Photo { get; set; }
        [NotMapped]
        public List<int> ReceptionDayId { get; set; }
        public List<Randevu> Randevus { get; set; }


    }
   
}
