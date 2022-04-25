using Dapper;
using System.Data;
using TestTask.Context;
using TestTask.Contracts;
using TestTask.Dto;
using TestTask.Entities;

namespace TestTask.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetEmployeesByCompanyId(int CompanyId)
        {
            var query = "SELECT * FROM Employees e INNER JOIN Passports p ON e.Id = p.EmployeeId " +
                "INNER JOIN Departaments d ON e.Id = d.EmployeeId";

            using (var connection = _context.CreateConnection())
            {
                var employeeDict = new Dictionary<int, Employee>();

                var employees = await connection.QueryAsync<Employee, Passport, Departament, Employee>(
                    query, (employee, passport, departament) =>
                    {
                        if (!employeeDict.TryGetValue(employee.Id, out var currentEmployee))
                        {
                            currentEmployee = employee;
                            employeeDict.Add(currentEmployee.Id, currentEmployee);
                        }

                        currentEmployee.Passport = passport;
                        currentEmployee.Departament = departament;
                        return currentEmployee;

                    }, splitOn: "EmployeeId"
                );

                var result = employees.Where(employees => employees.CompanyId == CompanyId).ToList();
                return result.ToList();
            }
        }

        public async Task<List<Employee>> GetEmployeesByDepartamentName(string DepartamentName)
        {
            var query = "SELECT * FROM Employees e INNER JOIN Passports p ON e.Id = p.EmployeeId " +
                "INNER JOIN Departaments d ON e.Id = d.EmployeeId";

            using (var connection = _context.CreateConnection())
            {
                var employeeDict = new Dictionary<int, Employee>();

                var employees = await connection.QueryAsync<Employee, Passport, Departament, Employee>(
                    query, (employee, passport, departament) =>
                    {
                        if (!employeeDict.TryGetValue(employee.Id, out var currentEmployee))
                        {
                            currentEmployee = employee;
                            employeeDict.Add(currentEmployee.Id, currentEmployee);
                        }

                        currentEmployee.Passport = passport;
                        currentEmployee.Departament = departament;
                        return currentEmployee;

                    }, splitOn: "EmployeeId"
                );

                var result = employees.Where(employees => employees.Departament.Name == DepartamentName).ToList();
                return result.ToList();
            }
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var query = "SELECT * FROM Employees WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, new { id });

                return employee;
            }
        }

        public async Task<Employee> CreateEmployee(EmployeeForCreationDto employee)
        {
            var query = "INSERT INTO Employees (Name, Surname, Phone, CompanyId) " +
                "VALUES (@Name, @Surname, @Phone, @CompanyId) " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", employee.Name, DbType.String);
            parameters.Add("Surname", employee.Surname, DbType.String);
            parameters.Add("Phone", employee.Phone, DbType.String);
            parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdEmployee = new Employee
                {
                    Id = id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Phone = employee.Phone,
                    CompanyId = employee.CompanyId,                    
                };

                return createdEmployee;
            }
        }

        public async Task UpdateEmployee(int id, EmployeeForUpdateDto employee)
        {
            var query = "UPDATE Employees SET Name = @Name, Surname = @Surname, Phone = @Phone, CompanyId = @CompanyId WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", employee.Name, DbType.String);
            parameters.Add("Surname", employee.Surname, DbType.String);
            parameters.Add("Phone", employee.Phone, DbType.String);
            parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteEmployee(int id)
        {
            var query = "DELETE FROM Employees WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        

    }
}
