using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

[assembly: AssemblyVersion("1.0.0.1")]

namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            // TODO : Add here the code that is called when the script is launched from Eclipse.
            string patientName = context.Patient.Name;
            var birthDate = string.IsNullOrEmpty(context.Patient.DateOfBirth.ToString()) ? "No Dob" : context.Patient.DateOfBirth.ToString();

            string planId = context.ExternalPlanSetup.Id;
            string maxDose = context.ExternalPlanSetup.Dose.DoseMax3D.Dose.ToString();
            string doseUnit = context.ExternalPlanSetup.Dose.DoseMax3D.UnitAsString;

            string locationV = context.ExternalPlanSetup.Dose.DoseMax3DLocation.x.ToString() + ", " +
                context.ExternalPlanSetup.Dose.DoseMax3DLocation.y + ", " +
                context.ExternalPlanSetup.Dose.DoseMax3DLocation.z;

            MessageBox.Show(string.Format("patient name = {0}, birthdate = {1}", patientName, birthDate));
            MessageBox.Show(string.Format("planID = {0}, max dose = {1} {2} at {3}", planId, maxDose, doseUnit, locationV));
        }
    }
}
