using HospitalSystem2.Models;
using System.Collections.Generic;

namespace HospitalSystem2.ViewModels
{
    public class DashboardVM
    {
        public Total Total { get; set; }
        public List<Profit> Profits { get; set; }
        public List<Cost> Costs { get; set; }
    }
}
