using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ExamplePlan.ViewModel
{
    public class PlanViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)

        {

            PropertyChanged?.Invoke(this, e);

        }

        IEnumerable<PlanSetup> _planSetups;

        PlanSetup _plan;
        User _user;

        private string planDetails;
        public string PlanDetails
        {
            get { return planDetails; }
            set
            {
                planDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanDetails"));
            }
        }

        public PlanViewModel(PlanSetup plan, User user, IEnumerable<PlanSetup> planSetups)
        {
            _planSetups = planSetups;
            _plan = plan;
            _user = user;
        }


        private void setText()
        {
            // Retrieve the count of plans displayed in Scope Window
            int scopePlanCount = _planSetups.Count();
            if (scopePlanCount == 0)
            {
                PlanDetails = "Scope Window does not contain any plans.";
                return;
            }

            // Retrieve names for different types of plans
            List<string> externalPlanIds = new List<string>();
            List<string> brachyPlanIds = new List<string>();
            List<string> protonPlanIds = new List<string>();
            foreach (var ps in _planSetups)
            {
                if (ps is BrachyPlanSetup)
                {
                    brachyPlanIds.Add(ps.Id);
                }
                else if (ps is IonPlanSetup)
                {
                    protonPlanIds.Add(ps.Id);
                }
                else
                {
                    externalPlanIds.Add(ps.Id);
                }
            }

            // Construct output message
            string message = string.Format("Hello {0}, the number of plans in Scope Window is {1}.",
              _user,
              scopePlanCount);
            if (externalPlanIds.Count > 0)
            {
                message += string.Format("\nPlan(s) {0} are external beam plans.", string.Join(", ", externalPlanIds));
            }
            if (brachyPlanIds.Count > 0)
            {
                message += string.Format("\nPlan(s) {0} are brachytherapy plans.", string.Join(", ", brachyPlanIds));
            }
            if (protonPlanIds.Count > 0)
            {
                message += string.Format("\nPlan(s) {0} are proton plans.", string.Join(", ", protonPlanIds));
            }

            // Display additional information. Use the active plan if available.
            PlanSetup plan = _plan != null ? _plan : _planSetups.ElementAt(0);
            message += string.Format("\n\nAdditional details for plan {0}:", plan.Id);
            // TODO Show fraction message
            message += string.Format("Number of fractions : {0}\n", plan.NumberOfFractions);
            message += string.Format("Dose per fraction : {0}\n", plan.PlannedDosePerFraction.ToString());

            // Access the structure set of the plan
            if (plan.StructureSet != null)
            {
                Image image = plan.StructureSet.Image;
                var structures = plan.StructureSet.Structures;
                message += string.Format("\n* Image ID: {0}", image.Id);
                message += string.Format("\n* Size of the Structure Set associated with the plan: {0}.\n", structures.Count());

                // TODO show structure info each
                foreach (var structureSingle in plan.StructureSet.Structures)
                {
                    // check validation
                    if (!structureSingle.IsEmpty)
                    {
                        message += string.Format("{0} in volume {1:F2} cc" + Environment.NewLine, structureSingle.Name, structureSingle.Volume);
                    }
                }
            }
            // message += string.Format("\n* Number of Fractions: {0}.", plan.NumberOfFractions);

            // Handle brachytherapy plans separately from external beam plans
            if (plan is BrachyPlanSetup)
            {
                BrachyPlanSetup brachyPlan = (BrachyPlanSetup)plan;
                var catheters = brachyPlan.Catheters;
                var seedCollections = brachyPlan.SeedCollections;
                message += string.Format("\n* Number of Catheters: {0}.", catheters.Count());
                message += string.Format("\n* Number of Seed Collections: {0}.", seedCollections.Count());
            }
            else
            {
                var beams = plan.Beams;
                message += string.Format("\n* Number of Beams: {0}." + Environment.NewLine, beams.Count());

                // TODO show info of beams
                foreach (Beam beamSingle in plan.Beams.OrderBy(x => x.BeamNumber))
                {
                    message += string.Format("Beam name equal to {0} , ", beamSingle.Id);
                    message += string.Format("and MeterSet = {0}" + Environment.NewLine, beamSingle.Meterset.Value.ToString() + " " + beamSingle.Meterset.Unit.ToString());
                }
            }
            if (plan is IonPlanSetup)
            {
                IonPlanSetup ionPlan = plan as IonPlanSetup;
                IonBeam beam = ionPlan.IonBeams.FirstOrDefault();
                if (beam != null)
                {
                    message += string.Format("\n* Number of Lateral Spreaders in first beam: {0}.", beam.LateralSpreadingDevices.Count());
                    message += string.Format("\n* Number of Range Modulators in first beam: {0}.", beam.RangeModulators.Count());
                    message += string.Format("\n* Number of Range Shifters in first beam: {0}.", beam.RangeShifters.Count());
                }
            }

            PlanDetails = message;

            // MessageBox.Show(message);
        }
    }
}
