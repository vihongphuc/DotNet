using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Models.DTO;
using UnitTest.Services;

namespace UnitTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        #region Property  
        private readonly IDepartmentService _departmentService;
        #endregion

        #region Constructor  
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        #endregion

        [HttpGet(nameof(GetName))]
        public async Task<string> GetName(int depId)
        {
            var result = await _departmentService.GetName(depId);
            return result;
        }

        [HttpGet(nameof(GetDetails))]
        public async Task<DepartmentDTO> GetDetails(int depId)
        {
            var result = await _departmentService.GetDetails(depId);
            return result;
        }

        [HttpDelete(nameof(Delete))]
        public async Task<bool> Delete(int depId, bool cascasde)
        {
            var result = await _departmentService.Delete(depId, cascasde);
            return result;
        }


    }
}
