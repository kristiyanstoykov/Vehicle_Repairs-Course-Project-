using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Vehicle_Repairs.Commands;
using Vehicle_Repairs.Database;
using Vehicle_Repairs.Model;
using Vehicle_Repairs.Observable;

namespace Vehicle_Repairs.ViewModel
{
    public class SearchViewModel : ObservableObject
    {
        private ObservableCollection<Vehicle> _vehicles;
        private ObservableCollection<Repair> _repairs;
        private string _repairedYear;
        private string _brand;
        private string _model;
        private DatabaseService dbService = new DatabaseService();

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand(SearchRepairs);
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new DelegateCommand(ClearSearch);
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

        public ObservableCollection<Repair> Repairs
        {
            get => _repairs;
            set
            {
                _repairs = value;
                RaisePropertyChangedEvent(nameof(Repairs));
            }
        }

        public string RepairedYear
        {
            get => _repairedYear;
            set
            {
                _repairedYear = value;
                RaisePropertyChangedEvent(nameof(RepairedYear));
                //SearchVehicles();
                //SearchRepairs();
            }
        }

        public string Brand
        {
            get => _brand;
            set
            {
                _brand = value;
                RaisePropertyChangedEvent(nameof(Brand));
                //SearchVehicles();
                //SearchRepairs();
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                RaisePropertyChangedEvent(nameof(Model));
                //SearchVehicles();
                //SearchRepairs();
            }
        }

        public SearchViewModel()
        {
            Vehicles = new ObservableCollection<Vehicle>();
            Repairs = new ObservableCollection<Repair>();
            LoadVehicles();
            LoadRepairs();
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
                // Handle exceptions (e.g., logging)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void LoadRepairs()
        {
            try
            {
                var records = dbService.GetAllRepairsWIthVehicles();
                foreach (var record in records)
                {
                    Repairs.Add(record);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void SearchRepairs()
        {
            Repairs = new ObservableCollection<Repair>(dbService.SearchRepairs(_repairedYear, _brand, _model));
        }

        private void ClearSearch()
        {
            RepairedYear = string.Empty;
            Brand = string.Empty;
            Model = string.Empty;
            Vehicles.Clear();
            Repairs.Clear();
            LoadVehicles();
            LoadRepairs();
        }


    }
}
