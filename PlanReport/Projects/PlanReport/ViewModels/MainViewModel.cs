using Dose_Metrics.ViewModels;
using DVH_Reporter.ViewModels;
using ExamplePlan.ViewModel;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PlanReport.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel(PlanViewModel planViewModel, DoseMetricViewModel doseMetricViewModel, DVHViewModel dVHViewModel)
        {
            this.PlanViewModel = planViewModel;
            this.DoseMetricViewModel = doseMetricViewModel;
            this.DVHViewModel = dVHViewModel;

            AddPrinter = new DelegateCommand<UserControl>(OnAddPrinter);
        }

        private void OnAddPrinter(UserControl obj)
        {
            PrintDialog print = new PrintDialog();
            print.PrintVisual(obj, "info");
        }   

        //private void OnAddPrinter(UserControl obj)
        //{
        //    PrintDialog print = new PrintDialog();
        //    print.PrintVisual(obj, "info");
        //}

        public DelegateCommand<UserControl> AddPrinter { get; private set; }

        public PlanViewModel PlanViewModel { get; set; }
        public DoseMetricViewModel DoseMetricViewModel {  get; set; }

        public DVHViewModel DVHViewModel { get; set; }
    }
}
