using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Models;
using UnitTest.Models.DTO;
using UnitTest.Services;

namespace UnitTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region Property  
        private readonly IEmployeeService _employeeService;
        #endregion

        #region Constructor  
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion

        [HttpGet(nameof(GetName))]
        public async Task<string> GetName(int employeeId)
        {
            var result = await _employeeService.GetById(employeeId);
            return result;
        }

        [HttpGet(nameof(GetDetails))]
        public async Task<EmployeeDTO> GetDetails(int employeeId)
        {
            var result = await _employeeService.GetDetails(employeeId);
            return result;
        }

    }
}
