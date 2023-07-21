using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HospitalSystem2.Enums
{
    public enum Reception
    {

        [Display(Name = "Bazar ertəsi")]
        Monday = 1,
        [Display(Name = "Çərşənbə axşamı")]
        Tuesday = 2,
        [Display(Name = "Çərşənbə")]
        Wednesday = 3,
        [Display(Name = "Cümə axşamı")]
        Thursday = 4,
        [Display(Name = "Cümə")]
        Friday = 5,
        [Display(Name = "Şənbə")]
        Saturday = 6,
        [Display(Name = "Bazar")]
        Sunday = 7,
    }
}
