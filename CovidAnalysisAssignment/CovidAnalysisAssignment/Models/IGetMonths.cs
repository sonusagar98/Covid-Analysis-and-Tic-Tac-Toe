using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Models
{
    /// <summary>
    /// for extracting months from the data available
    /// </summary>
    public interface IGetMonths
    {
        List<string> GetMonthsName(ObservableCollection<VaccinationData> data);
    }
}
