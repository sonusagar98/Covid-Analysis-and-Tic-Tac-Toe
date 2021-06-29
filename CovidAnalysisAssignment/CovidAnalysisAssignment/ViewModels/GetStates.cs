using CovidAnalysisAssignment.Interfaces;
using CovidAnalysisAssignment.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CovidAnalysisAssignment.Services

{ 
    public class GetStates : IGetStates
    {
        /// <summary>
        /// for extracting name of states for which data is available
        /// </summary>
        /// <param name="data">Raw data</param>
        /// <returns> List of string containing state name </returns>
        public List<string> GetStatesName(ObservableCollection<VaccinationData> data)
        {
            List<string> states = new List<string>();
            foreach (var d in data)
            {
                string state = d.State;
                if (states == null)
                    states.Add(state);
                else if (!states.Contains(state))
                    states.Add(state);
            }
            return states;
        }
    }
}
