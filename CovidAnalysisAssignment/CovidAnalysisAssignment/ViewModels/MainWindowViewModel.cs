using CovidAnalysisAssignment.Interfaces;
using CovidAnalysisAssignment.Model;
using CovidAnalysisAssignment.Models;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CovidAnalysisAssignment.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        #region ObservableCollection for ConfirmedCasesPoints, CuredCasesPoints, DeathCurvePoints, ActiveCurvePoints
        private ObservableCollection<DataPoint> _confirmedCasesPoints;
        public ObservableCollection<DataPoint> ConfirmedCasesPoints
        {
            get { return _confirmedCasesPoints; }
            set
            {
                _confirmedCasesPoints = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConfirmedCasesPoints"));
            }
        }

        private ObservableCollection<DataPoint> _curedCasesPoints;
        public ObservableCollection<DataPoint> CuredCasesPoints
        {
            get { return _curedCasesPoints; }
            set
            {
                _curedCasesPoints = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CuredCasesPoints"));


            }
        }

        private ObservableCollection<DataPoint> _deathCurvePoints;
        public ObservableCollection<DataPoint> DeathCurvePoints
        {
            get { return _deathCurvePoints; }
            set
            {
                _deathCurvePoints = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeathCurvePoints"));

            }
        }

        private ObservableCollection<DataPoint> _activeCurvePoints;
        public ObservableCollection<DataPoint> ActiveCurvePoints
        {
            get { return _activeCurvePoints; }
            set
            {
                _activeCurvePoints = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActiveCurvePoints"));

            }
        }
        #endregion

        //All data will be stored in TotalCovidStatus which is got from csv file
        public ObservableCollection<CovidStatusStateWise> TotalCovidStatus { get; set; }

        //Data which will be shown, (we can change month and this data will change to show data for that month 

        private ObservableCollection<CovidStatusStateWise> _covidStatusToDisplay;
        public ObservableCollection<CovidStatusStateWise> CovidStatusToDisplay
        {
            get { return _covidStatusToDisplay; }
            set { SetProperty(ref _covidStatusToDisplay, value); }
        }

        private void LoadCovidStatusToDisplay(string selectedMonth)
        {
            int MonthNumber = Array.IndexOf(monthDictionary,selectedMonth);
            CovidStatusToDisplay.Clear();
            foreach (CovidStatusStateWise value in TotalCovidStatus)
            {
                if (value.Date.Month == MonthNumber)
                {
                    CovidStatusToDisplay.Add(value);
                }
            }
        }

        #region Total cases (confirmed, active, recovered, deaths) in the graph for paricular state

        private int _totalActiveCases;
        public int totalActiveCases
        {
            get { return _totalActiveCases; }
            set
            {
                _totalActiveCases = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalActiveCases"));
            }
        }





        private int _totalConfirmedCases;

        public int totalConfirmedCases
        {
            get { return _totalConfirmedCases; }
            set
            {
                _totalConfirmedCases = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalConfirmedCases"));
            }
        }

        private int _totalRecoverdCases;

        public int totalRecoverdCases
        {
            get { return _totalRecoverdCases; }
            set
            {
                _totalRecoverdCases = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalRecoverdCases"));
            }
        }

        private int _totalDeathCases;

        public int totalDeathCases
        {
            get { return _totalDeathCases; }
            set
            {
                _totalDeathCases = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalDeathCases"));
            }
        }

        #endregion

        private void MakeGraphCordinates(string state)
        {


            ConfirmedCasesPoints.Clear();
            CuredCasesPoints.Clear();
            DeathCurvePoints.Clear();
            ActiveCurvePoints.Clear();

            foreach (CovidStatusStateWise value in TotalCovidStatus)
            {
                if (value.State == state)
                {
                    ConfirmedCasesPoints.Add(new DataPoint(DateTimeAxis.ToDouble(value.Date), value.Confirmed));
                    CuredCasesPoints.Add(new DataPoint(DateTimeAxis.ToDouble(value.Date), value.Cured));
                    DeathCurvePoints.Add(new DataPoint(DateTimeAxis.ToDouble(value.Date), value.Deaths));
                    ActiveCurvePoints.Add(new DataPoint(DateTimeAxis.ToDouble(value.Date), value.Active));
                    totalActiveCases = value.Active;
                    totalConfirmedCases = value.Confirmed;
                    totalRecoverdCases = value.Cured;
                    totalDeathCases = value.Deaths;

                }
            }
        }
        //for creating Model for my BARGRAPH "state vs Total Vaccination"
        private PlotModel _barModel;
        public PlotModel BarModel
        {
            get { return _barModel; }
            set {

                _barModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarModel"));
            }
        }

        // for storing data monthwise for every state 
        private ObservableCollection<stateVsTotalVaccination> _stateVsVaccination;
        public ObservableCollection<stateVsTotalVaccination> StateVsVaccination
        {
            get { return _stateVsVaccination; }
            set
            {
                _stateVsVaccination = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StateVsVaccination"));
            }
        }

        // for storing complete data from CSV FILE
        private ObservableCollection<VaccinationData> _vaccinationData;
        public ObservableCollection<VaccinationData> VaccinationData
        {
            get { return _vaccinationData; }
            set
            {
                _vaccinationData = value; 
            }
        }
        //public ObservableCollection<VaccinationData> VaccinationData { get; set; }


        //for storing data to display in the datagrid for selected month
        private ObservableCollection<VaccinationData> _vaccinationDataToDisplay;

        public ObservableCollection<VaccinationData> VaccinationDataToDisplay
        {
            get { return _vaccinationDataToDisplay; }
            set
            {
                _vaccinationDataToDisplay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VaccinationDataToDisplay"));
            }
        }

        // for creating a dictionary for accessing MONTH NAME using INDEX
        string[] monthDictionary;

        // Creating a dictionary to store area for respective states
        Dictionary<String, int> StatesArea;

        //for storing name of the MONTHS for which data is present and also to display in COMBOBOX
        private List<string> _months;
        public List<string> Months
        {
            get { return _months; }
            set { SetProperty(ref _months, value); }
        }

        // for getting the selected MONTH in COMBOBOX
        private string _selectedMonth;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMonth"));
                LoadVaccinatonDataToDisplay(Array.IndexOf(monthDictionary, _selectedMonth),DateTime.Parse("2021/01/01")); // for updating datagrid
                CreateBarmodel(Array.IndexOf(monthDictionary, _selectedMonth)); // for cahnging State Vs Total Vaccination
                LoadCovidStatusToDisplay(_selectedMonth);
            }
        }

        // for getting the selected date in datepicker
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDate"));
                LoadVaccinatonDataToDisplay(1, _selectedDate); // for updaating the datagrid (1 is passed just for sake of it It has no significance)
            }
        }

        // Interface for accessing GetStaes Class , AllocateArea class, GetMonths class, CheckHeader class
        private IGetStates _statesInterface;
        private IAllocateArea _allocateArea;
        private IGetMonths _getMonths;
        private ICheckHeader _checkHeader;

        //for storing name of the STATES for which data is present and also to display in COMBOBOX
        private List<string> _states;
        public List<string> States
        {
            get { return _states; }
            set { SetProperty(ref _states, value); }
        }


        // for getting the selected STATE in COMBOBOX
        private string _selectedState;
        public string SelectedState
        {
            get { return _selectedState; }
            set {
                _selectedState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedState"));
                Load_18_44_DaywiseStat(_selectedState);
                MakeGraphCordinates(_selectedState);
            }
        }

        // for displaying firstdose % in Progress Bar
        private int _firstDoseStateWise;
        public int FirstDoseStateWise
        {
            get { return _firstDoseStateWise; }
            set
            {
                _firstDoseStateWise = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstDoseStateWise"));
            }
        }

        // for displaying secondDose % in Progress Bar
        private int _secondDoseStateWise;
        public int SecondDoseStateWise
        {
            get { return _secondDoseStateWise; }
            set
            {
                _secondDoseStateWise = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SecondDoseStateWise"));
            }
        }

        // DELEGATE COMMAND for exporting data on clicking button "EXPORT"
        private DelegateCommand<string> text;
        public DelegateCommand<string> ExportData =>
            text ?? (text = new DelegateCommand<string>(ExecuteExportData, CanExportData));

        
        /// <summary>
        /// For Exporting Datagrid data in an excel file.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteExportData(string obj)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XLSX File|*.xlsx";
            saveFileDialog1.Title = "Save an excel File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
                worksheet.Name = "Vaccination Data";

                int i = 1;
                worksheet.Cells[i, 1] = "Date";
                worksheet.Cells[i, 2] = "State";
                worksheet.Cells[i, 3] = "Total Vaccination";
                worksheet.Cells[i, 4] = "First Dose";
                worksheet.Cells[i, 5] = "Second Dose";
                worksheet.Cells[i, 6] = "Covaxin";
                worksheet.Cells[i, 7] = "Covishield";
                worksheet.Cells[i, 8] = "18-44";
                worksheet.Cells[i, 9] = "45+";

                i = 2;
                int j = 1;

                foreach (var data in VaccinationDataToDisplay)
                {

                    //Type myType = typeof(VaccinationData);
                    //FieldInfo[] myfield = myType.GetFields();

                    worksheet.Cells[i, j] = data.DateTime.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data.State;
                    j += 1;
                    worksheet.Cells[i, j] = data.TotalVaccinated.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data.FirstDose.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data.SecondDose.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data.Covaxin.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data.Covishield.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data._18.ToString();
                    j += 1;
                    worksheet.Cells[i, j] = data._45.ToString();

                    i += 1;
                    j = 1;
                }

                workbook.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }
           
        }
        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanExportData(string arg)
        {
            return true;
        }


        private DelegateCommand<string> t;
        public DelegateCommand<string> ImportData =>
            t ?? (t = new DelegateCommand<string>(ExecuteImportData, CanImportData));

        

        private void ExecuteImportData(string obj)
        {
            String[] Header = new string[] {"Date","State","Total Vaccination","First Dose","Second Dose","Covaxin",
                "Covishield","18-44","45+"};
            OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "csv files (*.csv)|*.csv";

            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(openFileDlg.FileName, ".csv"));
                
                if (_checkHeader.IsHeaderValid(Header, lines[0]))
                {
                    VaccinationData.Clear();
                    for (int j = 1; j < lines.Length; j++)
                    {
                        string[] data = lines[j].Split(',');
                        for (int k = 0; k < data.Length; k++)
                        {
                            if (data[k] == "")
                            {
                                data[k] = "0";
                            }
                        }
                        if (Convert.ToInt32(data[2]) > 0)
                        {
                            VaccinationData vaccination = new VaccinationData(data[0], data[1], Convert.ToInt32(data[2]), Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), Convert.ToInt32(data[5]), Convert.ToInt32(data[6]), Convert.ToInt32(data[7]), Convert.ToInt32(data[8]));
                            VaccinationData.Add(vaccination);
                        }
                    }
                    LoadVaccinatonDataToDisplay(DateTime.Today.Month, _selectedDate);
                    Months.Clear();
                    Months = _getMonths.GetMonthsName(VaccinationData);
                    States.Clear();
                    States = _statesInterface.GetStatesName(VaccinationData);
                    LoadStateVsTotalVaccination();
                    CreateBarmodel(DateTime.Today.Month);
                    SelectedState = States[0];
                }

            }

           
        }
        private bool CanImportData(string arg)
        {
            return true;
        }

        private DelegateCommand<string> _importTab1;
        public DelegateCommand<string> ImportDataTab1 =>
            _importTab1 ?? (_importTab1 = new DelegateCommand<string>(ExecuteImportDataTab1, CanImportDataTab1));



        private void ExecuteImportDataTab1(string obj)
        {
            String[] Header = new string[] {"Date","State","Cured","Deaths","Confirmed"};
            OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "csv files (*.csv)|*.csv";

            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(openFileDlg.FileName, ".csv"));

                if (_checkHeader.IsHeaderValid(Header, lines[0]))
                {
                    TotalCovidStatus.Clear();
                    for (int j = 1; j < lines.Length; j++)
                    {
                        string[] currentData = lines[j].Split(',');
                        try
                        {
                            CovidStatusStateWise covidStatusStateWise = new CovidStatusStateWise(currentData[0], currentData[1],
                            Convert.ToInt32(currentData[2]), Convert.ToInt32(currentData[3]), Convert.ToInt32(currentData[4]));
                            TotalCovidStatus.Add(covidStatusStateWise);

                        }
                        catch
                        {

                        }
                        
                    }
                    LoadCovidStatusToDisplay("June");
                    Months.Clear();
                    Months = _getMonths.GetMonthsName(VaccinationData);
                    States.Clear();
                    States = _statesInterface.GetStatesName(VaccinationData);
                    SelectedState = States[0];
                    MakeGraphCordinates(SelectedState);
                }

            }


        }
        private bool CanImportDataTab1(string arg)
        {
            return true;
        }

        #region ObservableCollection for DatewiseStat_18, Covaxin_45, Covishield_45, FirstDose, Second Dose to display in line graphs
        //for storing DATEWISE STAT for 18-44 age group to display in line graph
        private ObservableCollection<DateValue> _datewiseStat_18;
        public ObservableCollection<DateValue> DatewiseStat_18
        {
            get { return _datewiseStat_18; }
            set
            {
                _datewiseStat_18 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DatewiseStat_18"));
            }
        }

        //foe storing DATEWISE STAT of Covaxin to display in line graph
        private ObservableCollection<DateValue> _covaxinType_45;
        public ObservableCollection<DateValue> Covaxin_45
        {
            get { return _covaxinType_45; }
            set
            {
                _covaxinType_45 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Covaxin_45"));
            }
        }

        //foe storing DATEWISE STAT of Covishield to display in line graph
        private ObservableCollection<DateValue> _covishieldType_45;
        public ObservableCollection<DateValue> Covishield_45
        {
            get { return _covishieldType_45; }
            set
            {
                _covishieldType_45 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Covishield_45"));
            }
        }

        //foe storing DATEWISE STAT of first Dose to display in line graph
        private ObservableCollection<DateValue> _firstDose;
        public ObservableCollection<DateValue> FirstDose
        {
            get { return _firstDose; }
            set
            {
                _firstDose = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstDose"));
            }
        }

        //foe storing DATEWISE STAT of Second Dose to display in line graph
        private ObservableCollection<DateValue> _secondDose;
        public ObservableCollection<DateValue> SecondDose
        {
            get { return _secondDose; }
            set
            {
                _secondDose = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SecondDose"));
            }
        } 
        #endregion

        public MainWindowViewModel(IGetStates st,IAllocateArea area, IGetMonths getMonths,ICheckHeader headerCheck)
        {
            monthDictionary = new string[13]{ "", "January", "February", "March", "April", "May", "June", "July", "August",
                    "September", "October", "November", "December"};
            _statesInterface = st;
            _allocateArea = area;
            _getMonths = getMonths;
            _checkHeader = headerCheck;
            _selectedMonth = monthDictionary[DateTime.Today.Month];
            _selectedDate = DateTime.Parse("2021/01/01");
            VaccinationData = new ObservableCollection<VaccinationData>();
            DatewiseStat_18 = new ObservableCollection<DateValue>();
            Covaxin_45 = new ObservableCollection<DateValue>();
            Covishield_45 = new ObservableCollection<DateValue>();
            FirstDose = new ObservableCollection<DateValue>();
            SecondDose = new ObservableCollection<DateValue>();
            VaccinationDataToDisplay = new ObservableCollection<VaccinationData>();
            StateVsVaccination = new ObservableCollection<stateVsTotalVaccination>();

            // TAB1
            TotalCovidStatus = new ObservableCollection<CovidStatusStateWise>();
            CovidStatusToDisplay = new ObservableCollection<CovidStatusStateWise>();
            ConfirmedCasesPoints = new ObservableCollection<DataPoint>();
            CuredCasesPoints = new ObservableCollection<DataPoint>();
            DeathCurvePoints = new ObservableCollection<DataPoint>();
            ActiveCurvePoints = new ObservableCollection<DataPoint>();
            //

            

            LoadVaccinatonData();
            LoadVaccinatonDataToDisplay(DateTime.Today.Month,_selectedDate);
            Months = new List<string>();
            Months = _getMonths.GetMonthsName(VaccinationData);
            States = new List<string>();
            States = _statesInterface.GetStatesName(VaccinationData);
            _selectedState = States[0];
            StatesArea = new Dictionary<string, int>();
            StatesArea = _allocateArea.AllocateStateArea();
            BarModel = new PlotModel();

            //TAB1
            LoadTotalCovidStatus();
            LoadCovidStatusToDisplay(_selectedMonth);
            MakeGraphCordinates(SelectedState);
            //
            LoadStateVsTotalVaccination();
            CreateBarmodel(DateTime.Today.Month);
            Load_18_44_DaywiseStat(_selectedState);
        }

        //for reading the csv file and storing data in VaccinationData

        private DelegateCommand<string> text2;
        public DelegateCommand<string> ExportDataTab1 =>
            text2 ?? (text2 = new DelegateCommand<string>(ExecuteExportDataTab1, CanExportDataTab1));


        //Logic for exporting data
        private void ExecuteExportDataTab1(string obj)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XLSX File|*.xlsx";
            saveFileDialog1.Title = "Save an excel File";
            saveFileDialog1.ShowDialog();
            MessageBox.Show(saveFileDialog1.FileName);
            if (saveFileDialog1.FileName != "")
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
                worksheet.Name = "Exported from gridview";

                int i = 1;
                worksheet.Cells[i, 1] = "Date";
                worksheet.Cells[i, 2] = "State";
                worksheet.Cells[i, 3] = "Cured";
                worksheet.Cells[i, 4] = "Deaths";
                worksheet.Cells[i, 5] = "Confirmed";

                i = 2;
                int j = 1;

                foreach (var data in CovidStatusToDisplay)
                {
                    try
                    {

                        worksheet.Cells[i, j] = data.Date.ToString();
                        j += 1;
                        worksheet.Cells[i, j] = data.State;
                        j += 1;
                        worksheet.Cells[i, j] = data.Cured.ToString();
                        j += 1;
                        worksheet.Cells[i, j] = data.Deaths.ToString();
                        j += 1;
                        worksheet.Cells[i, j] = data.Confirmed.ToString();
                    }
                    catch
                    {

                    }


                    i += 1;
                    j = 1;
                }

                workbook.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }

        }
        private bool CanExportDataTab1(string arg)
        {
            return true;
        }

        public void LoadTotalCovidStatus()
        {
            string filename = "StateWiseCovidData";
            string[] rows = File.ReadAllLines(System.IO.Path.ChangeExtension(filename, ".csv"));
            foreach (var row in rows)
            {
                string[] currentData = row.Split(',');
                try
                {
                    CovidStatusStateWise covidStatusStateWise = new CovidStatusStateWise(currentData[0], currentData[1],
                    Convert.ToInt32(currentData[2]), Convert.ToInt32(currentData[3]), Convert.ToInt32(currentData[4]));
                    TotalCovidStatus.Add(covidStatusStateWise);
                }
                catch
                {

                }
            }
        }

        //Tab 1 completed

        /// <summary>
        /// for extracting data from excel file on app startup and storing it in VaccinationData
        /// </summary>
        public void LoadVaccinatonData()
        {
            string filename = ConfigurationSettings.AppSettings["csvFile"];
            string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(filename, ".csv"));
            for (int i=1;i<lines.Length;i++)
            {
                string[] data = lines[i].Split(',');
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j] == "")
                    {
                        data[j] = "0";
                    }
                }
                if (Convert.ToInt32(data[2]) > 0)
                {
                    VaccinationData vaccination = new VaccinationData(data[0], data[1], Convert.ToInt32(data[2]), Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), Convert.ToInt32(data[5]), Convert.ToInt32(data[6]), Convert.ToInt32(data[7]), Convert.ToInt32(data[8]));
                    VaccinationData.Add(vaccination);
                }
            }
        }

        //for extracting data to display for selected month
        public void LoadVaccinatonDataToDisplay(int month, DateTime selectedDate)
        {
            VaccinationDataToDisplay.Clear();
            if (selectedDate == DateTime.Parse("2021/01/01"))
            {
                foreach (var data in VaccinationData)
                {
                    if (data.DateTime.Month == month)
                    {
                        VaccinationDataToDisplay.Add(data);
                    }
                }
            }
            else
            {
                foreach (var data in VaccinationData)
                {
                    if (data.DateTime == selectedDate)
                    {
                        VaccinationDataToDisplay.Add(data);
                    }
                }
            }
        }



        /// <summary>
        /// for storing stats monthwise for every state
        /// </summary>
        private void LoadStateVsTotalVaccination()
        {
            StateVsVaccination.Clear();

            for(int i=0;i< States.Count(); i++)
            {
                stateVsTotalVaccination curretData = new stateVsTotalVaccination(States[i]);
                for (int j=1;j<=Months.Count;j++) 
                {
                    DateTime lastDate = new DateTime(2021, j, 1).AddMonths(1).AddDays(-1);
                    if (j == 6)
                        lastDate = DateTime.Parse("2021/06/07");
                    VaccinationData listItem = VaccinationData.Single(a => a.DateTime == lastDate && a.State==States[i]);
                    curretData.TotalVaccination[j] = listItem.TotalVaccinated;
                    curretData.Age_18_44_Vaccination[j] = listItem._18;
                    curretData.Age_45_Vaccination[j] = listItem._45;
                    curretData.CovaxinDose[j] = listItem.Covaxin;
                    curretData.CovishieldDose[j] = listItem.Covishield;
                    if(j==1)
                        StateVsVaccination.Add(curretData);
                }
            }
        }

        /// <summary>
        /// Bar Model for displaying the BAR GRAPH of state vs Total Vaccination
        /// </summary>
        /// <param name="month">Selected Month for which bar graph is displayed</param>
        private void CreateBarmodel(int month)
        {

            BarModel.Series.Clear();
            BarModel.Axes.Clear();
            BarModel.Title = "State vs Total Vaccination";
            List<ColumnItem> totalCovaxinDose = new List<ColumnItem>();
            List<ColumnItem> totalCovishieldDose = new List<ColumnItem>();
            string[] stateName = new string[StateVsVaccination.Count];
            int i = 0;
            foreach (var f in StateVsVaccination)
            {
                totalCovaxinDose.Add(new ColumnItem { Value = f.CovaxinDose[month] });
                totalCovishieldDose.Add(new ColumnItem { Value = f.CovishieldDose[month] });
                stateName[i] = f.StateName;
                i += 1;
            }
            var covaxinBarSeries = new ColumnSeries
            {
                ItemsSource = totalCovaxinDose,             
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                FillColor = OxyColors.BlueViolet,
                Title = "Covaxin Dose"

            };
            var covishieldBarSeries = new ColumnSeries
            {
                ItemsSource = totalCovishieldDose,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                FillColor = OxyColors.Red,
                Title = "Covishield Dose"


            };
            BarModel.Series.Add(covaxinBarSeries);
            BarModel.Series.Add(covishieldBarSeries);
            BarModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Angle=60,
                ItemsSource = stateName

            });

            BarModel.InvalidatePlot(true);
        }

        /// <summary>
        /// for storing daywise stat for selected state
        /// </summary>
        /// <param name="selectedState">State for which data is to be displayed in the line graph</param>
        private void Load_18_44_DaywiseStat(string selectedState)
        {
            DatewiseStat_18.Clear();
            Covaxin_45.Clear();
            Covishield_45.Clear();
            FirstDose.Clear();
            SecondDose.Clear();
            FirstDoseStateWise = 0;
            SecondDoseStateWise = 0;
            int c = 0;
            VaccinationData prevData = null;
            foreach (var data in VaccinationData)
            {
                if (data.State==selectedState)
                {
                    DateTime dt;
                    var isValidDate = DateTime.TryParse("16/01/2021", out dt);
                    if (data.DateTime==dt.Date)
                        prevData = data;
                    else
                    {
                        DatewiseStat_18.Add(new DateValue(data.DateTime, data._18 - prevData._18));
                        Covaxin_45.Add(new DateValue(data.DateTime, data.Covaxin - prevData.Covaxin));
                        Covishield_45.Add(new DateValue(data.DateTime, data.Covishield - prevData.Covishield));
                        FirstDose.Add(new DateValue(data.DateTime, data.FirstDose - prevData.FirstDose));
                        SecondDose.Add(new DateValue(data.DateTime, data.SecondDose - prevData.SecondDose));
                        FirstDoseStateWise = data.FirstDose;
                        SecondDoseStateWise = data.SecondDose;
                        prevData = data;
                    }
                }
            }
            FirstDoseStateWise = FirstDoseStateWise * 100 / (200 * StatesArea[selectedState]);
            SecondDoseStateWise = SecondDoseStateWise * 100 / (200 * StatesArea[selectedState]);
        }
        protected void NotifyPropertyChange(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        }



    }
}
