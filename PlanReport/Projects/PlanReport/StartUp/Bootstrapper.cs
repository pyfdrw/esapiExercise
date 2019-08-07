using Autofac;
using Dose_Metrics.ESAPIServices;
using Dose_Metrics.ViewModels;
using DVH_Reporter.ViewModels;
using ExamplePlan.ViewModel;
using PlanReport.ViewModels;
using PlanReport.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanReport.StartUp
{
    public class Bootstrapper
    {
        public IContainer BootStrap(PlanSetup planSetup, User user, IEnumerable<PlanSetup> planSetups, ESAPI_Methods eSAPI_Methods)
        {
            var builder = new ContainerBuilder();

            // view
            // view model
            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<PlanViewModel>().AsSelf();
            builder.RegisterType<DoseMetricViewModel>().AsSelf();
            builder.RegisterType<DVHViewModel>().AsSelf();

            builder.RegisterInstance<PlanSetup>(planSetup);
            builder.RegisterInstance<User>(user);
            builder.RegisterInstance<IEnumerable<PlanSetup>>(planSetups);
            builder.RegisterInstance<ESAPI_Methods>(eSAPI_Methods);

            return builder.Build();
        }
    }
}
