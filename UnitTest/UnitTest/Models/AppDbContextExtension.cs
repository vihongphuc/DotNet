using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest.Models
{
    public static class AppDbContextExtension
    {
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }
        public static void EnsureSeeded(this AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Departments.Any())
            {
                var departs = new Department[]
                {
                    new Department { Name="IT", Desc="IT Departments." },
                    new Department { Name="FI", Desc="Accounting Departments." },
                    new Department { Name="HR", Desc="Human Resource Departments." },
                    new Department { Name="MR", Desc="Marketing Departments." }

                };
                context.AddRange(departs);
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT MyDB.Users OFF");
            }

            if (!context.Employees.Any())
            {
                var channels = new Employee[]
                {
                    new Employee{Name="Phuc", Desgination="HOD", DepartmentId=1},
                    new Employee{Name="Long", Desgination="Staff", DepartmentId=2},
                    new Employee{Name="Lam", Desgination="Assitant", DepartmentId=2},
                    new Employee{Name="An", Desgination="Deputy", DepartmentId=3},
                    new Employee{Name="Trong", Desgination="Staff", DepartmentId=4},
                };
                context.AddRange(channels);
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT MyDB.Users OFF");
            }

        }
    }
}
