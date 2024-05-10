using Vehicle_Repairs.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Repairs.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string databaseFile = "VehicleRepairs.db";
            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\.."));
            string databaseDir = Path.Combine(projectRoot, "db");

            if (!Directory.Exists(databaseDir))
            {
                Directory.CreateDirectory(databaseDir);
            }

            string databasePath = databaseDir + $"\\{databaseFile}";
            optionsBuilder.UseSqlite($"Data Source={databasePath}")
                .LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repair>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Vehicle>().Property(e => e.Id).ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship
            modelBuilder.Entity<Repair>()
                        .HasOne(r => r.Vehicle)
                        .WithMany(v => v.Repairs)
                        .HasForeignKey("VehicleId");

            modelBuilder.Entity<Repair>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Vehicle>().Property(e => e.Id).ValueGeneratedOnAdd();

            // Seed data for Vehicle
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Id = 1, Brand = "Toyota", Model = "Corolla", YearMade = 2018, RegistrationNumber = "XYZ123" },
                new Vehicle { Id = 2, Brand = "Honda", Model = "Civic", YearMade = 2020, RegistrationNumber = "ABC789" },
                new Vehicle { Id = 3, Brand = "Ford", Model = "Focus", YearMade = 2019, RegistrationNumber = "DEF456" },
                new Vehicle { Id = 4, Brand = "Chevrolet", Model = "Malibu", YearMade = 2017, RegistrationNumber = "GHI012" },
                new Vehicle { Id = 5, Brand = "BMW", Model = "3 Series", YearMade = 2021, RegistrationNumber = "JKL345" },
                // Additional vehicles
                new Vehicle { Id = 6, Brand = "Nissan", Model = "Altima", YearMade = 2018, RegistrationNumber = "NIS678" },
                new Vehicle { Id = 7, Brand = "Volkswagen", Model = "Golf", YearMade = 2019, RegistrationNumber = "VW901" },
                new Vehicle { Id = 8, Brand = "Hyundai", Model = "Elantra", YearMade = 2022, RegistrationNumber = "HYU234" },
                new Vehicle { Id = 9, Brand = "Mazda", Model = "CX-5", YearMade = 2021, RegistrationNumber = "MAZ567" },
                new Vehicle { Id = 10, Brand = "Mercedes", Model = "C-Class", YearMade = 2020, RegistrationNumber = "MER890" },
                // Additional Audi vehicles
                new Vehicle { Id = 11, Brand = "Audi", Model = "A4", YearMade = 2020, RegistrationNumber = "AUD100" },
                new Vehicle { Id = 12, Brand = "Audi", Model = "A6", YearMade = 2019, RegistrationNumber = "AUD200" },
                new Vehicle { Id = 13, Brand = "Audi", Model = "Q5", YearMade = 2021, RegistrationNumber = "AUD300" },
                new Vehicle { Id = 14, Brand = "Audi", Model = "Q7", YearMade = 2018, RegistrationNumber = "AUD400" },
                new Vehicle { Id = 15, Brand = "Audi", Model = "TT", YearMade = 2022, RegistrationNumber = "AUD500" },
                // Additional various brand vehicles
                new Vehicle { Id = 16, Brand = "Tesla", Model = "Model 3", YearMade = 2021, RegistrationNumber = "TES123" },
                new Vehicle { Id = 17, Brand = "Lexus", Model = "RX 350", YearMade = 2019, RegistrationNumber = "LEX456" },
                new Vehicle { Id = 18, Brand = "Porsche", Model = "Cayenne", YearMade = 2020, RegistrationNumber = "POR789" },
                new Vehicle { Id = 19, Brand = "Jaguar", Model = "F-Type", YearMade = 2022, RegistrationNumber = "JAG012" },
                new Vehicle { Id = 20, Brand = "Land Rover", Model = "Defender", YearMade = 2021, RegistrationNumber = "LAN345" },
                new Vehicle { Id = 21, Brand = "Kia", Model = "Sorento", YearMade = 2023, RegistrationNumber = "KIA678" },
                new Vehicle { Id = 22, Brand = "Subaru", Model = "Outback", YearMade = 2022, RegistrationNumber = "SUB901" },
                new Vehicle { Id = 23, Brand = "Volvo", Model = "XC90", YearMade = 2021, RegistrationNumber = "VOL234" },
                new Vehicle { Id = 24, Brand = "Mitsubishi", Model = "Outlander", YearMade = 2020, RegistrationNumber = "MIT567" },
                new Vehicle { Id = 25, Brand = "Fiat", Model = "500", YearMade = 2019, RegistrationNumber = "FIA890" }


            );

            // Seed data for Repair
            modelBuilder.Entity<Repair>().HasData(
                new Repair { Id = 1, VehicleId = 1, YearOfService = 2023, ServiceType = "Annual Maintenance", Description = "Full service including oil change, filters, and inspection" },
                new Repair { Id = 2, VehicleId = 1, YearOfService = 2023, ServiceType = "Tire Replacement", Description = "Replacement of all four tires" },
                new Repair { Id = 3, VehicleId = 1, YearOfService = 2022, ServiceType = "Brake Service", Description = "Brake pads and rotors replacement" },
                new Repair { Id = 4, VehicleId = 2, YearOfService = 2023, ServiceType = "Battery Replacement", Description = "Replacement of the car battery" },
                new Repair { Id = 5, VehicleId = 2, YearOfService = 2021, ServiceType = "AC Repair", Description = "Air conditioning system repair and coolant refill" },
                new Repair { Id = 6, VehicleId = 1, YearOfService = 2024, ServiceType = "Brake Service", Description = "Brake callipers replacement and brake fluid change" },
                new Repair { Id = 7, VehicleId = 3, YearOfService = 2024, ServiceType = "Battery Replacement", Description = "Battery check and replacement." },
                // Additional repairs
                new Repair { Id = 8, VehicleId = 4, YearOfService = 2021, ServiceType = "Oil Change", Description = "Engine oil and filter change" },
                new Repair { Id = 9, VehicleId = 5, YearOfService = 2022, ServiceType = "Transmission Repair", Description = "Transmission system check and overhaul" },
                new Repair { Id = 10, VehicleId = 6, YearOfService = 2023, ServiceType = "Windshield Replacement", Description = "Front windshield replacement due to damage" },
                new Repair { Id = 11, VehicleId = 7, YearOfService = 2021, ServiceType = "Suspension Repair", Description = "Suspension check and necessary repairs" },
                new Repair { Id = 12, VehicleId = 8, YearOfService = 2022, ServiceType = "Brake Service", Description = "Complete brake system overhaul" },
                new Repair { Id = 13, VehicleId = 9, YearOfService = 2023, ServiceType = "Exhaust Repair", Description = "Exhaust system repair and muffler replacement" },
                new Repair { Id = 14, VehicleId = 10, YearOfService = 2024, ServiceType = "Electrical Repair", Description = "Electrical system diagnostics and repairs" },
                // Corresponding repairs for Audi vehicles
                new Repair { Id = 15, VehicleId = 11, YearOfService = 2023, ServiceType = "Engine Diagnostic", Description = "Engine performance check and diagnostics" },
                new Repair { Id = 16, VehicleId = 11, YearOfService = 2022, ServiceType = "Oil Change", Description = "Replace engine oil and filter" },
                new Repair { Id = 17, VehicleId = 12, YearOfService = 2023, ServiceType = "Tire Replacement", Description = "Replace all four tires with new ones" },
                new Repair { Id = 18, VehicleId = 13, YearOfService = 2024, ServiceType = "Brake Service", Description = "Replace brake pads and inspect brake lines" },
                new Repair { Id = 19, VehicleId = 14, YearOfService = 2022, ServiceType = "AC Repair", Description = "Repair air conditioning and refill refrigerant" },
                new Repair { Id = 20, VehicleId = 15, YearOfService = 2023, ServiceType = "Suspension Repair", Description = "Inspect and repair suspension components" },
                // Corresponding repairs for various brand vehicles
                new Repair { Id = 21, VehicleId = 16, YearOfService = 2023, ServiceType = "Software Update", Description = "Full system software update" },
                new Repair { Id = 22, VehicleId = 17, YearOfService = 2022, ServiceType = "Transmission Service", Description = "Transmission inspection and fluid change" },
                new Repair { Id = 23, VehicleId = 18, YearOfService = 2023, ServiceType = "Performance Check", Description = "Complete performance diagnostics and tuning" },
                new Repair { Id = 24, VehicleId = 19, YearOfService = 2022, ServiceType = "Brake Service", Description = "Brake system overhaul" },
                new Repair { Id = 25, VehicleId = 20, YearOfService = 2023, ServiceType = "Suspension Check", Description = "Inspection and repair of suspension components" },
                new Repair { Id = 26, VehicleId = 21, YearOfService = 2024, ServiceType = "Oil Change", Description = "Engine oil replacement and system check" },
                new Repair { Id = 27, VehicleId = 22, YearOfService = 2023, ServiceType = "Tire Replacement", Description = "All tires replacement with alignment" },
                new Repair { Id = 28, VehicleId = 23, YearOfService = 2022, ServiceType = "Safety Check", Description = "Complete vehicle safety check and adjustments" },
                new Repair { Id = 29, VehicleId = 24, YearOfService = 2023, ServiceType = "AC Repair", Description = "Air conditioning system repair and coolant refill" },
                new Repair { Id = 30, VehicleId = 25, YearOfService = 2021, ServiceType = "Battery Replacement", Description = "Battery check and replacement" }
            );
        }
    }
}       
