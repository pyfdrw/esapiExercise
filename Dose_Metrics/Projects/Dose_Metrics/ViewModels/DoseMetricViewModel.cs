using Dose_Metrics.ESAPIServices;
using Dose_Metrics.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace Dose_Metrics.ViewModels
{
    public class DoseMetricViewModel : BindableBase
    {
        public List<string> Structures { get; set; }

        private string selectedStructure;
        public string SelectedStructure
        {
            get { return selectedStructure; }
            set
            {
                SetProperty(ref selectedStructure, value);
                AddMetric.RaiseCanExecuteChanged();
            }
        }

        public List<string> Metrics { get; set; }

        private ESAPI_Methods _esapi;
        private PlanSetup _planSetup;
        private string selectedMetric;

        public string SelectedMetric
        {
            get { return selectedMetric; }
            set
            {
                SetProperty(ref selectedMetric, value);
                AddMetric.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddMetric { get; private set; }
        public ObservableCollection<DVHMetric> DQPs { get; private set; }

        public DoseMetricViewModel(PlanSetup planSetup, ESAPI_Methods eSAPI_Methods)
        {
            _esapi = eSAPI_Methods;
            _planSetup = planSetup;
            Structures = new List<string>(
                _planSetup.StructureSet.Structures.Select(x => x.Id));

            Metrics = new List<string>(Enum.GetNames(typeof(MetricType)).Cast<string>());

            DQPs = new ObservableCollection<DVHMetric>();

            AddMetric = new DelegateCommand(OnAddMetric, CanAddMetric);
        }

        private void OnAddMetric()
        {
            DVHMetric dvhTmp = new DVHMetric()
            {
                StructureId = SelectedStructure,
                DoseMetric = SelectedMetric,
                OutputValue = "#N/A"
            };

            Structure s = _planSetup.StructureSet.Structures.FirstOrDefault(x => x.Id == SelectedStructure);
            DVHData dvhData = _esapi.GetDVH(_planSetup, s);

            if(dvhData != null)
            {
                switch (Metrics.IndexOf(SelectedMetric))
                {
                    case (int)MetricType.Max:
                        dvhTmp.OutputValue = dvhData.MaxDose.ToString();
                        break;
                    case (int)MetricType.Min:
                        dvhTmp.OutputValue = dvhData.MinDose.ToString();
                        break;
                    case (int)MetricType.Mean:
                        dvhTmp.OutputValue = dvhData.MeanDose.ToString();
                        break;
                    case (int)MetricType.Volume:
                        dvhTmp.OutputValue = s.Volume.ToString("F2") + " cc";
                        break;
                }

                DQPs.Add(dvhTmp);
            }
        }

        private bool CanAddMetric()
        {

            if (null == _planSetup || string.IsNullOrEmpty(SelectedMetric) || string.IsNullOrEmpty(selectedStructure))
            {
                return false;
            }
            else
                return true;
        }
    }
}
