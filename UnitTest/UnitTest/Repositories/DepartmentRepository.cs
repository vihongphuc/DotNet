using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Models;

namespace UnitTest.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
    }

    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context,
                                    ILogger<DepartmentRepository> logger) : base(context, logger)
        {
        }
    }
}
