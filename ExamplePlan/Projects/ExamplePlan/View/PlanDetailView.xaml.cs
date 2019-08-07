﻿using ExamplePlan.ViewModel;
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

namespace ExamplePlan.View
{
    /// <summary>
    /// Interaction logic for PlanDetailView.xaml
    /// </summary>
    public partial class PlanDetailView : UserControl
    {
        public PlanDetailView(PlanSetup plan, User user, IEnumerable<PlanSetup> planSetups)
        {
            PlanViewModel newPlanViewModel = new PlanViewModel(plan, user, planSetups);
            this.DataContext = newPlanViewModel;
            InitializeComponent();
        }
    }
}
