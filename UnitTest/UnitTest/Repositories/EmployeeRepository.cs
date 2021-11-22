using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Models;

namespace UnitTest.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
    }

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context,
                                    ILogger<EmployeeRepository> logger) : base(context, logger)
        {
            
        }

    }
}
