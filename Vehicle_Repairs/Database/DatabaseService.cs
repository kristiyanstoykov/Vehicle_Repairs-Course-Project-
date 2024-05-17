using Vehicle_Repairs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

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

        public IEnumerable<Repair> SearchRepairs(string year, string brand, string model, string repairDescription)
        {
            using (var context = new DatabaseContext())
            {
                var query = context.Repairs.AsQueryable();
                query = query.Include(c => c.Vehicle);

                // Convert year to an integer to perform comparison
                int searchYear;
                bool isYearValid = int.TryParse(year, out searchYear);
                bool yearApplied = false;

                // Apply filters for brand, model, and description
                if (!string.IsNullOrWhiteSpace(brand))
                {
                    query = query.Where(r => EF.Functions.Like(r.Vehicle.Brand, $"%{brand}%"));
                }

                if (!string.IsNullOrWhiteSpace(model))
                {
                    query = query.Where(r => EF.Functions.Like(r.Vehicle.Model, $"%{model}%"));
                }

                if (!string.IsNullOrWhiteSpace(repairDescription))
                {
                    query = query.Where(r => EF.Functions.Like(r.Description, $"%{repairDescription}%"));
                }

                // Apply year filter and determine the closest year if valid
                if (isYearValid)
                {
                    var filteredByYear = query.Where(r => r.YearOfService <= searchYear);
                    if (filteredByYear.Any())
                    {
                        yearApplied = true;
                        int closestYear = filteredByYear.OrderByDescending(r => r.YearOfService).First().YearOfService;
                        query = filteredByYear.Where(r => r.YearOfService == closestYear);
                    }
                }

                // If no year has been applied due to an invalid year or no results, handle by using the remaining filters
                if (!yearApplied)
                {
                    if (!query.Any()) return Enumerable.Empty<Repair>();  // Return an empty collection if no repairs are found
                    int closestYear = query.OrderByDescending(r => r.YearOfService).First().YearOfService;
                    query = query.Where(r => r.YearOfService == closestYear);
                }

                return query.ToList();
            }
        }

        public IEnumerable<T> Search<T>(
            List<Expression<Func<T, bool>>> stringFilters,
            Expression<Func<T, int>> yearExpr,
            int? searchYear,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includesExpr = null
        ) where T : class
        {
            using (var context = new DatabaseContext())
            {
                var query = context.Set<T>().AsQueryable();

                if (includesExpr != null)
                {
                    query = includesExpr(query);
                }

                foreach (var filter in stringFilters)
                {
                    query = query.Where(filter);
                }

                if (searchYear.HasValue)
                {
                    // This subquery fetches all years, then selects the closest one less than or equal to the search year
                    var yearQuery = query.Select(yearExpr);
                    var closestYear = yearQuery.Where(y => y <= searchYear.Value).OrderByDescending(y => y).FirstOrDefault();

                    // Ensure that a valid year was found, otherwise return an empty collection
                    if (closestYear == 0)
                    {
                        return Enumerable.Empty<T>();
                    }
                    
                    var parameter = yearExpr.Parameters[0];
                    var equalsClosestYear = Expression.Equal(yearExpr.Body, Expression.Constant(closestYear, typeof(int)));
                    var lambda = Expression.Lambda<Func<T, bool>>(equalsClosestYear, parameter);
                    query = query.Where(lambda);
                }

                return query.ToList();
            }
        }

        public void AddRepair(Repair repair, Vehicle vehicle)
        {
            using (var context = new DatabaseContext())
            {
                var existingVehicle = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == vehicle.RegistrationNumber);
                if (existingVehicle == null)
                {
                    context.Vehicles.Add(vehicle);
                    context.SaveChanges();
                }
                else
                {
                    vehicle = existingVehicle;
                }
                
                repair.VehicleId = vehicle.Id;
                context.Repairs.Add(repair);
                context.SaveChanges();
            }
        }
    }
}
