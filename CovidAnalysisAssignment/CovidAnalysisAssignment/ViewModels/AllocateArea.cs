using CovidAnalysisAssignment.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.ViewModels
{
    /// <summary>
    /// Storing area of all the states and UT in a dictionary
    /// </summary>
    public class AllocateArea : IAllocateArea
    {

        /// <returns> Dictionary<"State Name", Area> containing area </returns>
        public Dictionary<string, int> AllocateStateArea()
        {
            Dictionary<string, int> area = new Dictionary<string, int>();
            string filename = "State Area";
            string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(filename, ".csv"));
            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                area.Add(data[0], Convert.ToInt32(data[1]));
            }
            return area;
        }
    }
}
