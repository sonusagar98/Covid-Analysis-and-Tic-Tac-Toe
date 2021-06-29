using CovidAnalysisAssignment.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Interfaces
{
    /// <summary>
    /// for extracting name of states and UT from data available
    /// </summary>
    public interface IGetStates
    {
        List<string> GetStatesName(ObservableCollection<VaccinationData> data);
    }
}
