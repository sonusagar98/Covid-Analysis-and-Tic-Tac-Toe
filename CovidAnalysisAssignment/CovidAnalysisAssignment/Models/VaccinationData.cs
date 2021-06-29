using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Models
{
    public class VaccinationData
    {
        /// <summary>
        /// for storing the raw data
        /// </summary>
        public DateTime DateTime { get; set; }
        public string State { get; set; }
        public int TotalVaccinated { get; set; }
        public int FirstDose { get; set; }
        public int SecondDose { get; set; }
        public int Covaxin { get; set; }
        public int Covishield { get; set; }
        public int _18 { get; set; }
        public int _45 { get; set; }

        public VaccinationData(string dateTime, string state, int totalVaccinated, int firstDose, int secondDose, int covaxin, int covishield, int age_18, int age_45)
        {
            DateTime dt;
            var isValidDate = DateTime.TryParse(dateTime, out dt);
            if (isValidDate)
                DateTime = dt.Date;
            State = state;
            TotalVaccinated = totalVaccinated;
            FirstDose = firstDose;
            SecondDose = secondDose;
            Covaxin = covaxin;
            Covishield = covishield;
            _18 = age_18;
            _45 = age_45;
        }
    }
}
