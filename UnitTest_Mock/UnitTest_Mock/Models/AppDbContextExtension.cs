using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest_Mock.Models
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

            // Look for any students.
            if (!context.Employees.Any())
            {
                var channels = new Employee[]
                {
                    new Employee{Name="Phuc", Desgination="HOD"},
                    new Employee{Name="Long", Desgination="Staff"},
                    new Employee{Name="Lam", Desgination="Assitant"},
                    new Employee{Name="An", Desgination="Deputy"},
                    new Employee{Name="Trong", Desgination="Staff"},
                };
                //foreach (var s in channels)
                //{
                //    context.Channels.Add(s);
                //}
                context.AddRange(channels);
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT MyDB.Users OFF");
            }

        }
    }
}
