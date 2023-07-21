using System.ComponentModel.DataAnnotations;

namespace HospitalSystem2.ViewModels
{
    public class ResetPasswordVM
    {
       
        
        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }
        
        

    }
}
    

