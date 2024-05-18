using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vehicle_Repairs.Commands;
using Vehicle_Repairs.Database;
using Vehicle_Repairs.Model;
using Vehicle_Repairs.Observable;

namespace Vehicle_Repairs.ViewModel
{
    public class AddRepairViewModel : ObservableObject
    {
        private readonly MainViewModel _mainViewModel;
        private string _brand;
        private string _model;
        private string _repairDescription;
        private string _repairedYear;
        private string _registrationNumber;
        private string _yearMade;
        private DatabaseService dbService = new DatabaseService();

        public AddRepairViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public string Brand
        {
            get => _brand;
            set
            {
                _brand = value;
                RaisePropertyChangedEvent(nameof(Brand));
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                RaisePropertyChangedEvent(nameof(Model));
            }
        }

        public string RepairDescription
        {
            get => _repairDescription;
            set
            {
                _repairDescription = value;
                RaisePropertyChangedEvent(nameof(RepairDescription));
            }
        }

        public string RepairedYear
        {
            get => _repairedYear;
            set
            {
                _repairedYear = value;
                RaisePropertyChangedEvent(nameof(RepairedYear));
            }
        }

        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                _registrationNumber = value;
                RaisePropertyChangedEvent(nameof(RegistrationNumber));
            }
        }

        public string YearMade 
        {
            get => _yearMade;
            set
            {
                _yearMade = value;
                RaisePropertyChangedEvent(nameof(YearMade));
            }
        }

        public ICommand AddRepairCommand
        {
            get
            {
                return new DelegateCommand(AddRepair);
            }
        }

        private void AddRepair()
        {
            Repair repair = new Repair
            {
                ServiceType = "",
                Description = RepairDescription,
                YearOfService = int.Parse(RepairedYear),
            };

            Vehicle vehicle = new Vehicle
            {
                Brand = Brand,
                Model = Model,
                RegistrationNumber = RegistrationNumber
            };

            dbService.AddRepair(repair, vehicle);

            Clear();
        }

        public void Clear()
        {
            _mainViewModel.Clear();
        }

        public void ClearProps()
        {
            RepairedYear = string.Empty;
            Brand = string.Empty;
            Model = string.Empty;
            RepairDescription = string.Empty;
            RegistrationNumber = string.Empty;
            YearMade = string.Empty;
        }
    }
}
