using TestTask.Dto;
using TestTask.Entities;

namespace TestTask.Contracts
{
    public interface IEmployeeRepository
    {
        public Task<Employee> GetEmployee(int id);
        public Task<Employee> CreateEmployee(EmployeeForCreationDto employee);
        public Task UpdateEmployee(int id, EmployeeForUpdateDto employee);
        public Task DeleteEmployee(int id);
        public Task<List<Employee>> GetEmployeesByCompanyId(int CompanyId);
        public Task<List<Employee>> GetEmployeesByDepartamentName(string DepartamentName);
    }
}
