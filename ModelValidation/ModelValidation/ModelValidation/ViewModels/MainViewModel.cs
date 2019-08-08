using ModelValidation.Models;
using OxyPlot;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // ?
        private ICommand closedWindowCommand;

        public ICommand ClosedWindowCommand
        {
            get { return closedWindowCommand; }
            set { SetProperty(ref closedWindowCommand, value); }
        }


        private VMS.TPS.Common.Model.API.Application app;
        private Patient _patient;
        private Course _course;
        private PlanSetup _planSetup;

        public MainViewModel(VMS.TPS.Common.Model.API.Application app)
        {
            this.app = app;
            Courses = new ObservableCollection<string>();
            PlanSetups = new ObservableCollection<string>();

            ScanDataCollection = new ObservableCollection<ScanData>();

            OpenPatientCommand = new DelegateCommand(OnOpenPatient);

            ClosedWindowCommand = new DelegateCommand(ClosedWindowAction);
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
            return new ScanData { Direction = "N/A" };
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
