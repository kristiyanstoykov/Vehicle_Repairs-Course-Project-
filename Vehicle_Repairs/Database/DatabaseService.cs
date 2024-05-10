using Vehicle_Repairs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Vehicle_Repairs.Database
{
    public class DatabaseService
    {
        public DatabaseService()
        {
            using (var context = new DatabaseContext())
            {
                var exists = context.Database.CanConnect();
                if (!exists)
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
            }
        }

        public List<T> GetAll<T>() where T : class
        {
            using (var context = new DatabaseContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public void Add<T>(T entity) where T : class
        {
            using (var context = new DatabaseContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public List<Vehicle> GetAllVehicles()
        {
            using (var context = new DatabaseContext())
            {
                return context.Vehicles.Include(c => c.Repairs).ToList();
            }
        }

        public List<Repair> GetAllRepairs()
        {
            return GetAll<Repair>().ToList();
        }

        public List<Repair> GetAllRepairsWIthVehicles()
        {
            using (var context = new DatabaseContext())
            {
                return context.Repairs.Include(r => r.Vehicle).ToList();
            }
        }

        public IEnumerable<Repair> SearchRepairs(string year, string brand, string model)
        {
            using (var context = new DatabaseContext())
            {
                var query = context.Repairs.AsQueryable();
                query = query.Include(c => c.Vehicle);

                // Convert year to an integer to perform comparison
                int searchYear;
                bool isYearValid = int.TryParse(year, out searchYear);

                if (isYearValid)
                {
                    // Filter the results by the closest year less than or equal to the given year if the exact match is not found
                    var closestYearQuery = query.Where(c => c.YearOfService <= searchYear)
                                                .OrderByDescending(c => c.YearOfService);
                    if (closestYearQuery.Any())
                    {
                        // Update the query to limit it to the closest year available
                        int closestYear = closestYearQuery.FirstOrDefault().YearOfService;
                        query = closestYearQuery.Where(c => c.YearOfService == closestYear);
                    }
                }

                if (!string.IsNullOrWhiteSpace(brand))
                {
                    query = query.Where(c => EF.Functions.Like(c.Vehicle.Brand, $"%{brand}%"));
                }

                if (!string.IsNullOrWhiteSpace(model))
                {
                    query = query.Where(c => EF.Functions.Like(c.Vehicle.Model, $"%{model}%"));
                }

                return query.ToList();
            }
        }

    }
}
