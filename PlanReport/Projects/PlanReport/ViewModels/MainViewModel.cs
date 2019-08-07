using Dose_Metrics.ViewModels;
using DVH_Reporter.ViewModels;
using ExamplePlan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanReport.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(PlanViewModel planViewModel, DoseMetricViewModel doseMetricViewModel, DVHViewModel dVHViewModel)
        {
            this.PlanViewModel = planViewModel;
            this.DoseMetricViewModel = doseMetricViewModel;
            this.DVHViewModel = dVHViewModel;
        }

        public PlanViewModel PlanViewModel { get; set; }
        public DoseMetricViewModel DoseMetricViewModel {  get; set; }

        public DVHViewModel DVHViewModel { get; set; }
    }
}
