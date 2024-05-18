using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Repairs.Observable;

namespace Vehicle_Repairs.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private bool _isAddRepairVisible = false;
        public SearchViewModel SearchViewModel { get; set; }
        public AddRepairViewModel AddRepairViewModel { get; set; }

        public MainViewModel()
        {
            SearchViewModel = new SearchViewModel(this);
            AddRepairViewModel = new AddRepairViewModel(this);
        }

        public bool IsAddRepairVisible
        {
            get => _isAddRepairVisible;
            set
            {
                _isAddRepairVisible = value;
                RaisePropertyChangedEvent(nameof(IsAddRepairVisible));
            }
        }

        public void UpdateAddRepairProps(string serviceYear, string brand, string model, string repairDescription)
        {
            AddRepairViewModel.RepairedYear = serviceYear;
            AddRepairViewModel.Brand = brand;
            AddRepairViewModel.Model = model;
            AddRepairViewModel.RepairDescription = repairDescription;
        }

        public void Clear()
        {
            HideAddRepairControl();
            SearchViewModel.ClearSearchProps();
            AddRepairViewModel.ClearProps();
        }

        public void ShowAddRepairControl()
        {
            IsAddRepairVisible = true;
        }

        public void HideAddRepairControl()
        {
            IsAddRepairVisible = false;
        }
    }
}
