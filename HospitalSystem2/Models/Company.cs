using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem2.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
