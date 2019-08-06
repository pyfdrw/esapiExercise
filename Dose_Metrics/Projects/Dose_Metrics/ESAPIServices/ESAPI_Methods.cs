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

    }
}
