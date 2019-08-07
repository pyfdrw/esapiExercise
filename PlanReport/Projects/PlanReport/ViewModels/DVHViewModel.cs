using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace DVH_Reporter.ViewModels
{

    public class DVHViewModel
    {
        public PlotModel MyPlotModel { get; private set; }

        PlanSetup _planSetup = null;

        public DVHViewModel(PlanSetup planSetup)
        {
            _planSetup = planSetup;

            MyPlotModel = new PlotModel()
            {
                Title = $"DVH for {_planSetup.Id}",
                LegendTitle = "Structure Id"
            };

            DrawDVH();
        }

        // Draw DVH
        private void DrawDVH()
        {
            if (null != _planSetup.StructureSet)
            {
                foreach (var item in
                    _planSetup.StructureSet.Structures.Where(
                        x => x.DicomType != "MARKER" && x.DicomType != "SUPPORT" && x.HasSegment))
                {
                    // get dvh
                    DVHData dvhData = _planSetup.GetDVHCumulativeData(
                        item, VMS.TPS.Common.Model.Types.DoseValuePresentation.Absolute, VMS.TPS.Common.Model.Types.VolumePresentation.Relative, 1);

                    // draw dvh to MyPlotModel
                    LineSeries series = new LineSeries { Title = $"{item.Id}" };
                    foreach (var dataPoint in dvhData.CurveData)
                    {
                        series.Points.Add(new DataPoint(dataPoint.DoseValue.Dose, dataPoint.Volume));
                    }

                    MyPlotModel.Series.Add(series);
                }
            }
            else
            {
                throw new Exception("Empty StructureSet");
            }
        }
    }
}
