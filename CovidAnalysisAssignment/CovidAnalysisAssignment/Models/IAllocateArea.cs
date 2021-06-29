using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Models
{
    /// <summary>
    /// for allocating area to states
    /// </summary>
    public interface IAllocateArea
    {
        Dictionary<String, int> AllocateStateArea();
    }
}
