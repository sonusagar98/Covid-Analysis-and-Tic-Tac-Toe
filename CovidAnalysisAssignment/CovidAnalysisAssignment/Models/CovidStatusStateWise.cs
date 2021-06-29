using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAnalysisAssignment.Model
{
    /// <summary>
    /// this class is datamodel for taking input data from csv file
    /// Contains confirmed, cured and death cases for mentioned date and state in a row. 
    /// </summary>
    public class CovidStatusStateWise : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public string State { get; set; }


        public int ConfirmedAndCured { get; set; }

        private int _cured;
        public int Cured
        {
            get { return _cured; }
            set
            {

                _cured = value;
                OnPropertyChanged("Cured");
                OnPropertyChanged("Confirmed");
                OnPropertyChanged("Active");
            }
        }



        public int Deaths { get; set; }


        private int _confirmed;
        public int Confirmed
        {
            get { return ConfirmedAndCured - Cured; }
            set
            {
                _confirmed = value;
                OnPropertyChanged("Confirmed");
                OnPropertyChanged("Active");
            }
        }

        private int _active;

        public int Active
        {
            get { return Confirmed - Cured; }
            set
            {
                _active = value;
                OnPropertyChanged("Active");
            }
        }





        public CovidStatusStateWise(string date, string state, int cured, int deaths, int confirmed)
        {
            this.Date = (DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture)).Date;
            this.State = state;
            Cured = cured;
            this.Deaths = deaths;

            this.ConfirmedAndCured = confirmed + cured;
            Confirmed = this.ConfirmedAndCured - cured;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
