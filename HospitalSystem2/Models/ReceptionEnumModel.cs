namespace HospitalSystem2.Models
{
    public class ReceptionEnumModel
    {
        public int Id { get; set; }
        public int ReceptionDayId { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }
    }
}
