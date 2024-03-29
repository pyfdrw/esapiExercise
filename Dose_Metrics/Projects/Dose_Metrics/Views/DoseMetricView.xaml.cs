﻿using Dose_Metrics.ESAPIServices;
using Dose_Metrics.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;

namespace Dose_Metrics.Views
{
    /// <summary>
    /// Interaction logic for DoseMetricView.xaml
    /// </summary>
    public partial class DoseMetricView : UserControl
    {
        public DoseMetricView(PlanSetup planSetup, ESAPI_Methods eSAPI_Methods)
        {
            DoseMetricViewModel doseMetricViewModel = new DoseMetricViewModel(planSetup, eSAPI_Methodss);

            this.DataContext = doseMetricViewModel;
            InitializeComponent();
        }
    }
}
