using AutoMapper;
using System.Threading.Tasks;
using UnitTest.Models;
using UnitTest.Models.DTO;
using UnitTest.Repositories;

namespace UnitTest.Services
{
    public interface IEmployeeService
    {
        Task<string> GetById(int EmpID);
        Task<EmployeeDTO> GetDetails(int EmpID);

        Task<bool> Insert(EmployeeDTO Emp);
        Task<bool> Update(EmployeeDTO Emp);
        Task<bool> Delete(int EmpID);
    }

    public class EmployeeService : IEmployeeService
    {
        #region Property  

        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Constructor  
        public EmployeeService(IMapper mapper,
                                IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        #endregion

        public async Task<string> GetById(int EmpID)
        {
            var dbEmployee = await GetDetails(EmpID);
            return dbEmployee.Name;
        }

        public async Task<EmployeeDTO> GetDetails(int EmpID)
        {
            var dbEmployee = await _employeeRepository.FindById(EmpID);
            return _mapper.Map<EmployeeDTO>(dbEmployee);
        }

        public async Task<bool> Insert(EmployeeDTO Emp)
        {
            var succeed = false;
            var dbEmp = _mapper.Map<Employee>(Emp);
            try
            {
                succeed = await _employeeRepository.Add(dbEmp);
            }
            catch
            {
            }

            return succeed;
        }

        public async Task<bool> Update(EmployeeDTO Emp)
        {
            var succeed = false;
            try
            {
                var dbEmmp = await _employeeRepository.FindById(Emp.Id);
                if (dbEmmp != null)
                {
                    dbEmmp.Name = Emp.Name;
                    dbEmmp.Desgination = Emp.Desgination;

                    succeed = await _employeeRepository.Update(dbEmmp);
                }
            }
            catch
            {
            }

            return succeed;
        }

        public async Task<bool> Delete(int EmpID)
        {
            bool succeed = false;
            try
            {
                var dbEmmp = await _employeeRepository.FindById(EmpID);
                succeed = await _employeeRepository.Remove(dbEmmp);
            }
            catch
            {
            }

            return succeed;
        }
    }
}
