using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Models
{
    public class stateVsTotalVaccination
    {
        /// <summary>
        /// for storing Vaccination Details monthwise
        /// </summary>
        public int[] TotalVaccination { get; set; }
        public int[] Age_18_44_Vaccination { get; set; }
        public int[] Age_45_Vaccination { get; set; }
        public int[] CovaxinDose { get; set; }
        public int[] CovishieldDose { get; set; }
        public string StateName { get; set; }


        public stateVsTotalVaccination(string state)
        {
            TotalVaccination = new int[13];
            Age_18_44_Vaccination = new int[13];
            Age_45_Vaccination = new int[13];
            CovaxinDose = new int[13];
            CovishieldDose = new int[13];
            StateName = state;
        }
    }
}
