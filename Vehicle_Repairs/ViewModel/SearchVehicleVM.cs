using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vehicle_Repairs.Commands;
using Vehicle_Repairs.Database;
using Vehicle_Repairs.Model;
using Vehicle_Repairs.Observable;

namespace Vehicle_Repairs.ViewModel
{
    public class SearchVehicleVM : ObservableObject
    {
        private readonly MainViewModel _mainViewModel;
        private ObservableCollection<Vehicle> _vehicles;
        private string _yearMade;
        private string _brand;
        private string _model;
        private string _registrationNumber;
        private bool _isVehiclesEmpty = false;
        private DatabaseService dbService = new DatabaseService();

        public SearchVehicleVM(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Vehicles = new ObservableCollection<Vehicle>();
            LoadVehicles();
        }

        public SearchVehicleVM()
        {
            Vehicles = new ObservableCollection<Vehicle>();
            LoadVehicles();
        }

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand(Search);
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new DelegateCommand(ClearProps);
            }
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set
            {
                _vehicles = value;
                RaisePropertyChangedEvent(nameof(Vehicles));
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

        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                _registrationNumber = value;
                RaisePropertyChangedEvent(nameof(RegistrationNumber));
            }
        }

        public bool IsVehiclesEmpty
        {
            get => _isVehiclesEmpty;
            set
            {
                _isVehiclesEmpty = value;
                RaisePropertyChangedEvent(nameof(IsVehiclesEmpty));
            }
        }

        private void LoadVehicles()
        {
            try
            {
                var records = dbService.GetAllVehicles();
                foreach (var record in records)
                {
                    Vehicles.Add(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void Search()
        {
            var stringFilters = new List<Expression<Func<Vehicle, bool>>>();
            int? searchYear = string.IsNullOrWhiteSpace(YearMade) ? null : int.Parse(YearMade);
            Expression<Func<Vehicle, int>> yearExpr = v => v.YearMade;

            if (!string.IsNullOrWhiteSpace(Brand))
            {
                stringFilters.Add(v => EF.Functions.Like(v.Brand, $"%{Brand}%"));
            }

            if (!string.IsNullOrWhiteSpace(Model))
            {
                stringFilters.Add(v => EF.Functions.Like(v.Model, $"%{Model}%"));
            }

            if (!string.IsNullOrWhiteSpace(RegistrationNumber))
            {
                stringFilters.Add(v => EF.Functions.Like(v.RegistrationNumber, $"%{RegistrationNumber}%"));
            }

            Func<IQueryable<Vehicle>, IIncludableQueryable<Vehicle, object>> include = query => query.Include(v => v.Repairs);

            Vehicles = new ObservableCollection<Vehicle>(dbService.Search<Vehicle>(stringFilters, yearExpr, searchYear, include));

            IsVehiclesEmpty = Vehicles.Count == 0;
        }

        public void ClearProps()
        {
            Brand = string.Empty;
            Model = string.Empty;
            RegistrationNumber = string.Empty;
            YearMade = string.Empty;
            Vehicles.Clear();
            LoadVehicles();
            IsVehiclesEmpty = false;
        }
    }
}
