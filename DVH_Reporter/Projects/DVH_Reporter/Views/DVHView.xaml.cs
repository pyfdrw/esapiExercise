using DVH_Reporter.ViewModels;
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

namespace DVH_Reporter.Views
{
    /// <summary>
    /// Interaction logic for DVHView.xaml
    /// </summary>
    public partial class DVHView : UserControl
    {
        public DVHView(PlanSetup planSetup)
        {
            DVHViewModel dvhViewModel = new DVHViewModel(planSetup);

            this.DataContext = dvhViewModel;

            InitializeComponent();
        }
    }
}
