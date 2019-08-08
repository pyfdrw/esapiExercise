using ModelValidation.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

[assembly : ESAPIScript(IsWriteable = true)]
namespace ModelValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VMS.TPS.Common.Model.API.Application _app;

        public MainWindow()
        {
            _app = VMS.TPS.Common.Model.API.Application.CreateApplication(); // 15.5
            //_app = VMS.TPS.Common.Model.API.Application.CreateApplication(null, null);  // 13.x

            var mainViewModel = new MainViewModel(_app);

            this.DataContext = mainViewModel;

            // this.Closing += MainWindow_Closing;

            InitializeComponent();
        }

        //private void MainWindow_Closing(object sender, CancelEventArgs e)
        //{
        //    _app.ClosePatient();
        //    _app.Dispose(); // ?
        //}

    }
}
