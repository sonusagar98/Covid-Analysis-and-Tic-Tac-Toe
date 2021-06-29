using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Models
{
    /// <summary>
    /// for checking if header present in importedd file is valid or not
    /// </summary>
    public interface ICheckHeader
    {
        bool IsHeaderValid(string[] Header,string fileHeader);
    }
}
