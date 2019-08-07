using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using DVH_Reporter.Views;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext context , System.Windows.Window window/*, ScriptEnvironment environment*/)
        {
            // TODO : Add here the code that is called when the script is launched from Eclipse.
            if(context?.PlanSetup?.Dose == null)
            {
                MessageBox.Show("Please select one plan");
            }

            DVHView dvhView = new DVHView(context.PlanSetup);

            window.Content = dvhView;
            window.Width = 700;
            window.Height = 500;
            window.Title = "DVH Plot";


            //
            OxyPlot.Wpf.BarSeries bs = new OxyPlot.Wpf.BarSeries();
        }
    }
}
