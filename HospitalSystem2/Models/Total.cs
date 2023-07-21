using System;

namespace HospitalSystem2.Models
{
    public class Total
    {
        public int Id { get; set; }
        public double LastModifiedAmount { get;set; }
       public string LastModifiedDescription { get; set; }
        
        public DateTime LastModifiedTime { get;set; }   
        public double TotalCash { get; set; } 
        public string LastModifiedBy { get; set; }
        
    }
}
