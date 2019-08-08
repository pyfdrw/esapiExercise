using ModelValidation.Models;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace ModelValidation.ViewModels
{
    // Views
    public class MainViewModel : BindableBase
    {
        private string patientId;

        public string PatientId
        {
            get { return patientId; }
            set { SetProperty(ref patientId, value); } // prism
        }
        public DelegateCommand OpenPatientCommand { get; private set; }
        public ObservableCollection<string> Courses { get; private set; }
        private string selectedCourse;

        public string SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                SetProperty(ref selectedCourse, value);
                AddPlanSetups();
            }
        }

        public PlotModel MyPlotModel { get; private set; }

        public ObservableCollection<string> PlanSetups { get; private set; }
        private string selectedPlan;

        public ObservableCollection<ScanData> ScanDataCollection { get; private set; }
        public DelegateCommand ExportDataCommand { get; private set; }

        // ?
        private ICommand closedWindowCommand;

        public ICommand ClosedWindowCommand
        {
            get { return closedWindowCommand; }
            set { SetProperty(ref closedWindowCommand, value); }
        }

        // ViewModel and visibility
        private Visibility profileVisibility = Visibility.Collapsed;
        public Visibility ProfileVisibility
        {
            get { return profileVisibility; }
            set
            {
                SetProperty(ref profileVisibility, value);
            }
        }

        private Visibility pddVisibility = Visibility.Collapsed;
        public Visibility PDDVisibility
        {
            get { return pddVisibility; }
            set
            {
                SetProperty(ref pddVisibility, value);
            }
        }

        private PlotModel profilePlotModel = new PlotModel()
        {
            Title = "Profile (x)"
        };
        public PlotModel ProfilePlotModel
        {
            get { return profilePlotModel; }
            set
            {
                SetProperty(ref profilePlotModel, value);
            }
        }

        private PlotModel pddPlotModel = new PlotModel()
        {
            Title = "PDD"
        };
        public PlotModel PDDPlotModel
        {
            get { return pddPlotModel; }
            set
            {
                SetProperty(ref pddPlotModel, value);
            }
        }

        private VMS.TPS.Common.Model.API.Application app;
        private Patient _patient;
        private Course _course;
        private PlanSetup _planSetup;

        public DelegateCommand<object> SelectedItemChangedCommand { get; set; }

        public MainViewModel(VMS.TPS.Common.Model.API.Application app)
        {
            this.app = app;
            Courses = new ObservableCollection<string>();
            PlanSetups = new ObservableCollection<string>();

            ScanDataCollection = new ObservableCollection<ScanData>();

            OpenPatientCommand = new DelegateCommand(OnOpenPatient);

            ClosedWindowCommand = new DelegateCommand(ClosedWindowAction);

            SelectedItemChangedCommand = new DelegateCommand<object>(executeMethod: PlotFunction);

            ExportDataCommand = new DelegateCommand(OnExportData, CanExportData);
        }

        
        private bool CanExportData()
        {
            return !string.IsNullOrEmpty(SelectedPlan) && ScanDataCollection.Count != 0;
        }

        private void OnExportData()
        {
            foreach (var item in ScanDataCollection.GroupBy(x => x.Direction))
            {
                if(item.Key == "PDD")
                {
                    // Export PDD
                    ExportFiles(item.ToArray(), true);
                }
                else
                {
                    foreach (var item2 in ScanDataCollection.GroupBy(x => x.FieldSize))
                    {
                        ExportFiles(item2.ToArray(), false);
                    }
                }
            }
        }

        private void ExportFiles(ScanData[] scanData, bool isPdd)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{scanData.First().Direction}_" + 
                $"{(isPdd ? "All FS" : "FS_" + scanData.First().FieldSize.ToString())}" +
                ".csv";

            using (StreamWriter sw = new StreamWriter(Path.Combine(path, fileName)))
            {
                // Field Size, Header line
                sw.WriteLine($"Pos, " +
                    $"{(isPdd ? string.Join(", ", scanData.Select(x => x.FieldSize)) : string.Join(", ", scanData.Select(x => x.Depth)))}");
                for (int i = 0; i < scanData.First().DataPoints.Count; i++)
                {
                    sw.WriteLine($"{scanData.First().DataPoints[i].Position:F2}, " 
                        + $"{string.Join(", ", scanData.Select(x => x.DataPoints[i].Dose))}");
                }

                sw.Flush();
            }
        }

        private bool CanPlot(object arg)
        {
            ScanData sd = arg as ScanData;
            return null == sd ? false : true;
        }

        private void PlotFunction(object obj)
        {
            if (!CanPlot(obj))
            {
                return;
            }
            ScanData sd = obj as ScanData;
            if(sd.Direction == "PDD")
            {
                ProfileVisibility = Visibility.Collapsed;
                PDDVisibility = Visibility.Visible;
                PDDPlotModel.Series.Clear();

                LineSeries ls = new LineSeries();
                foreach (var item in sd.DataPoints)
                {
                    ls.Points.Add(new DataPoint(item.Position, item.Dose));
                }
                PDDPlotModel.Series.Add(ls);

                PDDPlotModel.InvalidatePlot(true);

            }
            else if (sd.Direction == "Profile (x)")
            {
                PDDVisibility = Visibility.Collapsed;
                ProfileVisibility = Visibility.Visible;
                ProfilePlotModel.Series.Clear();

                LineSeries ls = new LineSeries();
                foreach (var item in sd.DataPoints)
                {
                    ls.Points.Add(new DataPoint(item.Position, item.Dose));
                }

                ProfilePlotModel.Series.Add(ls);
                ProfilePlotModel.InvalidatePlot(true);
            }
        }


        private void ClosedWindowAction()
        {
            app.ClosePatient();
            app.Dispose();
        }

        private void OnOpenPatient()
        {
            // 
            if (!string.IsNullOrEmpty(PatientId))
            {
                Courses.Clear();
                PlanSetups.Clear();

                app.ClosePatient(); // 
                _patient = app.OpenPatientById(PatientId); // Can only open one

                if(null == _patient)
                {
                    MessageBox.Show($"{PatientId} not found;");
                    return;
                }
                else
                {
                    Courses.AddRange(_patient.Courses.Select(x => x.Id).ToArray());
                    // foreach (var item in _patient.Courses)
                    // {
                    //     Courses.Add(item.Id);
                    // }
                }
            }
        }

        public string SelectedPlan
        {
            get { return selectedPlan; }
            set
            {
                SetProperty(ref selectedPlan, value);
                CalculateDoseProfiles();
            }
        }

        private void CalculateDoseProfiles()
        {
            ScanDataCollection.Clear();

            //
            _planSetup = _course.PlanSetups.SingleOrDefault(x => x.Id == SelectedPlan);
            if(null != _planSetup)
            {
                double[] depth = new double[] { 15, 50, 100, 200, 300};  // mm

                foreach (Beam beam in _planSetup.Beams)
                {
                    // Get 5 profile and 1 pdd
                    foreach (var dd in depth)
                    {
                        ScanDataCollection.Add(GetProfileAtDepth(beam, dd));
                    }
                    ScanDataCollection.Add(GetPDD(beam));
                }
            }

        }

        private ScanData GetProfileAtDepth(Beam beam, double dd)
        {
            ScanData scanData = new ScanData()
            {
                FieldSize = GetFieldSize(beam),
                Depth = dd,
                Direction = "Profile (x)"
            };

            VVector startPos = new VVector((-scanData.FieldSize / 2 ) * 1.5, dd - 200, 0);

            VVector endPos = new VVector((scanData.FieldSize / 2) * 1.5, dd - 200, 0);

            double[] size = new double[(int)(scanData.FieldSize / 2 * 3)];

            DoseProfile dp = beam.Dose.GetDoseProfile(startPos, endPos, size);

            foreach (var pd in dp)
            {
                scanData.DataPoints.Add(new ScanDataPoint()
                {
                    Position = pd.Position.x,
                    Dose = pd.Value
                });
            }

            ExportDataCommand.RaiseCanExecuteChanged();

            return scanData;
        }

        private ScanData GetPDD(Beam beam)
        {
            double FS = GetFieldSize(beam);

            ScanData scanData = new ScanData()
            {
                FieldSize = FS,
                Direction = "PDD"
            };

            VVector startPos = new VVector(0, -100, 0);

            VVector endPos = new VVector(0, 100, 0);

            double[] size = new double[301];

            DoseProfile dp = beam.Dose.GetDoseProfile(startPos, endPos, size);

            foreach (var pd in dp)
            {
                scanData.DataPoints.Add(new ScanDataPoint()
                {
                    Position = pd.Position.y + 200,
                    Dose = pd.Value
                });
            }

            ExportDataCommand.RaiseCanExecuteChanged();
            SelectedItemChangedCommand.RaiseCanExecuteChanged();


            return scanData;
        }

        private double GetFieldSize(Beam beam)
        {
            double x1 = beam.ControlPoints.First().JawPositions.X1;
            double x2 = beam.ControlPoints.First().JawPositions.X2;

            return x2 - x1;
        }

        private void AddPlanSetups()
        {
            // 
            PlanSetups.Clear();

            if (!string.IsNullOrEmpty(SelectedCourse))
            {
                _course = _patient.Courses.SingleOrDefault(x => x.Id == SelectedCourse);
                if(null != _course)
                {
                    PlanSetups.AddRange(
                        _course.PlanSetups.Select(x => x.Id));
                }
            }
        }

    }
}
