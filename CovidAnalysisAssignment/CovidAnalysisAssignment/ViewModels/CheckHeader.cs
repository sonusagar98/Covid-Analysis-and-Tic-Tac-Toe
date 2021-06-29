using CovidAnalysisAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CovidAnalysisAssignment.ViewModels
{
    public class CheckHeader : ICheckHeader
    {
        public bool IsHeaderValid(string[] Header, string fileHeader)
        {
            string[] data = fileHeader.Split(',');
            int i = 0;
            for (i = 0; i < Header.Length; i++)
            {
                if (Header[i] != data[i])
                {
                    MessageBox.Show("Header is not correct", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }
    }
}
