using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dose_Metrics.Models
{
    public enum MetricType
    {
        Min,
        Max,
        Mean,
        Volume,
        VolA_at_DoseA,
        VolR_at_DoseA,
        DoseA_at_VolumeR,
        DoseR_at_VolumeR
    }
}
