using CovidAnalysisAssignment.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.ViewModels
{
    public class GetMonths : IGetMonths
    {    
        /// <summary>
        /// for getting months for which data is available
        /// </summary>
        /// <param name="data">Complete data</param>
        /// <returns></returns>

        public List<string> GetMonthsName(ObservableCollection<VaccinationData> data)
        {
            string[] monthDictionary = new string[13]{ "", "January", "February", "March", "April", "May", "June", "July", "August",
                    "September", "October", "November", "December"};
            List<string> Months = new List<string>();
            foreach (var d in data)
            {
                int monthIndex = d.DateTime.Month;
                if (!Months.Contains(monthDictionary[monthIndex]))
                    Months.Add(monthDictionary[monthIndex]);
            }
            return Months;
        }
    }
}
