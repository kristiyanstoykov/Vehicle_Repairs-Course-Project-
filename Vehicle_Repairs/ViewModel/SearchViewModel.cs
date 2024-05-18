using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
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
        private readonly MainViewModel _mainViewModel;
        private ObservableCollection<Vehicle> _vehicles;
        private ObservableCollection<Repair> _repairs;
        private string _repairedYear;
        private string _brand;
        private string _model;
        private string _repairDescription;
        private DatabaseService dbService = new DatabaseService();
        private bool _isRepairsEmpty = false;


        public SearchViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Repairs = new ObservableCollection<Repair>();
            LoadRepairs();
        }

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
                return new DelegateCommand(Clear);
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

        public string RepairDescription
        {
            get => _repairDescription;
            set
            {
                _repairDescription = value;
                RaisePropertyChangedEvent(nameof(RepairDescription));
            }
        }

        public bool IsRepairsEmpty
        {
            get => _isRepairsEmpty;
            set
            {
                _isRepairsEmpty = value;
                RaisePropertyChangedEvent(nameof(IsRepairsEmpty));
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
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void SearchRepairs2()
        {
            Repairs = new ObservableCollection<Repair>(dbService.SearchRepairs(_repairedYear, _brand, _model, _repairDescription));

            IsRepairsEmpty = Repairs.Count == 0;
        }

        private void SearchRepairs()
        {
            var stringFilters = new List<Expression<Func<Repair, bool>>>();
            int? searchYear = string.IsNullOrWhiteSpace(RepairedYear) ? null : int.Parse(RepairedYear);
            Expression<Func<Repair, int>> yearExpr = r => r.YearOfService;
            Func<IQueryable<Repair>, IIncludableQueryable<Repair, object>> include = query => query.Include(r => r.Vehicle);

            if (!string.IsNullOrWhiteSpace(Brand))
            {
                stringFilters.Add(r => EF.Functions.Like(r.Vehicle.Brand, $"%{Brand}%"));
            }

            if (!string.IsNullOrWhiteSpace(Model))
            {
                stringFilters.Add(r => EF.Functions.Like(r.Vehicle.Model, $"%{Model}%"));
            }

            if (!string.IsNullOrWhiteSpace(RepairDescription))
            {
                stringFilters.Add(r => EF.Functions.Like(r.Description, $"%{RepairDescription}%"));
            }

            Repairs = new ObservableCollection<Repair>(dbService.Search<Repair>(stringFilters, yearExpr, searchYear, include));

            IsRepairsEmpty = Repairs.Count == 0;
            if (IsRepairsEmpty)
            {
                _mainViewModel.UpdateAddRepairProps(RepairedYear, Brand, Model, RepairDescription);
                _mainViewModel.ShowAddRepairControl();
            }
        }

        public void ClearSearchProps()
        {
            RepairedYear = string.Empty;
            Brand = string.Empty;
            Model = string.Empty;
            RepairDescription = string.Empty;
            IsRepairsEmpty = false;
            Repairs.Clear();
            LoadRepairs();
        }

        public void Clear()
        {
            _mainViewModel.HideAddRepairControl();
            _mainViewModel.Clear();
        }

    }
}
