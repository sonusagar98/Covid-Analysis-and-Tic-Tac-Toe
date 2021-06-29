using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Models
{
    /// <summary>
    /// for storing values datewise for different field for displaying in line graph
    /// </summary>
    public class DateValue
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }

        public DateValue(DateTime dt,int v)
        {
            Date = dt;
            Value = v;
        }
    }
}
