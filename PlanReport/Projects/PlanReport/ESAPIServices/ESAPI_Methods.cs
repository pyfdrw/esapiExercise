using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Dose_Metrics.ESAPIServices
{
    public class ESAPI_Methods
    {
        internal DVHData GetDVH(PlanSetup planSetup, Structure s)
        {
            return planSetup.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 1);
        }

        internal double GetVolume(PlanSetup planSetup, Structure s, DoseValue doseValue, bool isVolumeAbsolute)
        {
            var vol = isVolumeAbsolute ? planSetup.GetVolumeAtDose(s, doseValue, VolumePresentation.AbsoluteCm3)
                :
                planSetup.GetVolumeAtDose(s, doseValue, VolumePresentation.Relative);
            return vol;
        }

        internal string GetDoseWithUnit(PlanSetup planSetup, Structure s, double volumeVolue, bool isDoseAbsolute)
        {
            var doseV = isDoseAbsolute ?
                planSetup.GetDoseAtVolume(s, volumeVolue, VolumePresentation.Relative, DoseValuePresentation.Absolute) :
                planSetup.GetDoseAtVolume(s, volumeVolue, VolumePresentation.Relative, DoseValuePresentation.Relative);

            return doseV.ToString();
        }
    }
}
