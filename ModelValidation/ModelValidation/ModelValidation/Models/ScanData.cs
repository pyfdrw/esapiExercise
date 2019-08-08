using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelValidation.Models
{
    public class ScanData
    {
        public double FieldSize { get; set; }
        public double  Depth { get; set; }
        public string Direction { get; set; }
        
        public List<ScanDataPoint> DataPoints { get; set; }

        public ScanData()
        {
            DataPoints = new List<ScanDataPoint>();
        }
    }
}
