using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using VMS.TPS.Common.Model.Types;

namespace Example_DVH
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {

        ObservableCollection<string> organNames = null;
        PlanSetup planSetupInUse = null;

        Structure stuSelected = null;

        public bool IsInitializationFinished = false;

        public MainControl(ObservableCollection<string> organList, PlanSetup planIn)
        {
            if(null == organList || organList.Count == 0 || planIn == null)
            {
                IsInitializationFinished = false;
                // MessageBox.Show("Unable to init");
            }
            else
            {
                organNames = organList;
                planSetupInUse = planIn;

                IsInitializationFinished = true;
            }

            // MessageBox.Show("before to init");
            InitializeComponent();
            // organListComboBox.ItemsSource = organNames;
        }

        public void DrawDVH()
        {
            if (stuSelected == null)
                return;

            DVHData dvhData = planSetupInUse.GetDVHCumulativeData(stuSelected,
                                          DoseValuePresentation.Absolute,
                                          VolumePresentation.Relative, 1);

            // clear 
            MainCanvas.Children.Clear();

            // Calculate multipliers for scaling DVH to canvas.
            double xCoeff = MainCanvas.Width / dvhData.MaxDose.Dose;
            double yCoeff = MainCanvas.Height / 100;

            // Set Y axis label
            DoseMaxLabel.Content = string.Format("{0:F0} {1}", dvhData.MaxDose.Dose, dvhData.MaxDose.UnitAsString);

            // Draw histogram 
            for (int i = 0; i < dvhData.CurveData.Length - 1; i++)
            {
                // Set drawing line parameters
                var line = new Line() { Stroke = new SolidColorBrush(stuSelected.Color), StrokeThickness = 2.0 };

                // Set line coordinates
                line.X1 = dvhData.CurveData[i].DoseValue.Dose * xCoeff;
                line.X2 = dvhData.CurveData[i + 1].DoseValue.Dose * xCoeff;
                // Y axis start point is top-left corner of window, convert it to bottom-left.
                line.Y1 = MainCanvas.Height - dvhData.CurveData[i].Volume * yCoeff;
                line.Y2 = MainCanvas.Height - dvhData.CurveData[i + 1].Volume * yCoeff;
                // Add line to the existing canvas
                MainCanvas.Children.Add(line);
            }

            // show info 
            infoText.Text += string.Format(stuSelected.Id + "  Max dose = {0:F2} {1}, mean dose = {2:F2} {3}, min dose = {4:F2} {5}" + Environment.NewLine,
                dvhData.MaxDose.Dose, dvhData.MaxDose.UnitAsString, 
                dvhData.MeanDose.Dose, dvhData.MeanDose.UnitAsString,
                dvhData.MinDose.Dose, dvhData.MinDose.UnitAsString);
        }

        // calculate dvh from organ Id
        private int calculateDvhFromOrganId()
        {
            if(organListComboBox.SelectedIndex != -1)
            {
                var stru =  planSetupInUse.StructureSet.Structures.Where(x => x.Id.Equals(organListComboBox.SelectedItem as string)).First();
                if (stru == null)
                    throw new Exception("Could not determin the organ by delected id");

                stuSelected = stru;

                return 0;
            }
            else
            {
                MessageBox.Show("Please selected one structure");
                return 1;
            }
        }

        // DVH calculation
        private void calDvhClick(object sender, RoutedEventArgs e)
        {
            calculateDvhFromOrganId();
            DrawDVH();
        }
    }
}
