using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelValidation.Models
{
    public class ScanDataPoint
    {
        private double position;

        public double Position
        {
            get { return position; }
            set { position = value; }
        }

        public double Dose { get; set; }
    }
}
