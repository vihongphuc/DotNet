using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Models;

namespace UnitTest.Test.Repositories
{
    public class GenericRepository
    {
        Mock<DbSet<Department>> _mockDepartmentDBSet;
        Mock<DbSet<Employee>> _mockEmployeeDBSet;
        Mock<AppDbContext> _mockContext;

        ILogger<UnitTest.Repositories.GenericRepository<Department>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<AppDbContext>();
            _mockDepartmentDBSet = new Mock<DbSet<Department>>();            

            //var mock = new Mock<ILogger<GenericRepository>>();
            //_logger = mock.Object;
            //or use this short equivalent 
            _logger = Mock.Of<ILogger<UnitTest.Repositories.GenericRepository<Department>>>();
        }

        [TestMethod]
        public void Remove_TestClassObjectPassed_ProperMethodCalled()
        {
            var departments = new List<Department> { 
                new Department{ Id=1, Name="test 1" }
            };
            // Arrange
            _mockContext.Setup(s => s.Departments).Returns(_mockDepartmentDBSet.Object);
            //mockContext.Setup(x => x.Set<Department>()).Returns(_mockDepartmentDBSet.Object);
            //_mockDepartmentDBSet.Setup(x => x.Remove(It.IsAny<Department>())).Returns();

            _mockContext
                .Setup(m => m.Departments.Remove(It.IsAny<Department>()));
                //.Callback<Department>((entity) => _mockContext.Remove(entity));


            // Act
            UnitTest.Repositories.IGenericRepository<Department> repository = new UnitTest.Repositories.GenericRepository<Department>(_mockContext.Object, _logger);
            var dept = repository.FindById(1);
            //repository.Remove(testObject);

            //Assert

            //context.Verify(x => x.Set<TestClass>());
            //dbSetMock.Verify(x => x.Remove(It.Is<TestClass>(y => y == testObject)));
        }
    }
}
