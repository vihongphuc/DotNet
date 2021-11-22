using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitTest.Models;
using UnitTest.Models.DTO;
using UnitTest.Repositories;

namespace UnitTest.Services
{
    public interface IDepartmentService
    {
        Task<string> GetName(int deptId);
        Task<DepartmentDTO> GetDetails(int deptId);

        Task<bool> Delete(int deptId, bool includedEmployee = false);
    }

    public class DepartmentService : IDepartmentService
    {
        #region Property  

        private readonly IMapper _mapper;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Department> _departmentRepository;

        #endregion

        #region Constructor  
        public DepartmentService(IMapper mapper,
                                IGenericRepository<Employee> employeeRepository,
                                IGenericRepository<Department> departmentRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        #endregion

        public async Task<string> GetName(int deptId)
        {
            var dbEmployee = await GetDetails(deptId);
            return dbEmployee.Name;
        }

        public async Task<DepartmentDTO> GetDetails(int deptId)
        {
            var dbEmployee = await _departmentRepository.FindById(deptId);
            return _mapper.Map<DepartmentDTO>(dbEmployee);
        }

        public async Task<bool> Delete(int deptId, bool includedEmployee = false)
        {
            bool succeed = false;
            Expression<Func<Employee, bool>> predicate = (emp) => emp.DepartmentId == deptId;

            var dbDepart = await _departmentRepository.FindById(deptId);
            using (var trans = await _departmentRepository.BeginTransactionAsync())
            {
                try
                {
                    var dbEmployees = await _employeeRepository.Find(predicate);
                    if (dbEmployees.Count() > 0)
                    {
                        if (includedEmployee)
                            succeed = await _employeeRepository.RemoveRange(dbEmployees, notSave: true);
                        else
                            succeed = false;
                    }

                    if (succeed)
                    {
                        succeed = await _departmentRepository.Remove(dbDepart);
                        await trans.CommitAsync();
                    }
                }
                catch
                {
                    succeed = false;
                    await trans.RollbackAsync();
                }
            }

            return succeed;
        }
    }
}
