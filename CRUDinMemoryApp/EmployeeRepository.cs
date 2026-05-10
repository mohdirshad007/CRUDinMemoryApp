using CRUDinMemoryApp;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class EmployeeRepository
{
    private static readonly List<Employee> _employees = new List<Employee>()
    {
        new Employee { ID = 1, Name = "John Doe", Salary = 50000 },
        new Employee { ID = 2, Name = "Alice Smith", Salary = 60000 },
        new Employee { ID = 3, Name = "Bob Johnson", Salary = 55000 },
        new Employee { ID = 4, Name = "Carol White", Salary = 52000 },
        new Employee { ID = 5, Name = "David Brown", Salary = 65000 }
    };

    private readonly IDistributedCache _cache;

    public EmployeeRepository(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    // GET all employees
    public IEnumerable<Employee> GetAll()
    {
        var cached = _cache.GetString(AllEmployeesKey);
        if (cached != null)
        {
            return JsonSerializer.Deserialize<List<Employee>>(cached);
        }
        _cache.SetString(AllEmployeesKey, JsonSerializer.Serialize(_employees),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        return _employees;
    }

    // GET employee by ID
    public Employee GetById(int id)
    {
        return _employees.FirstOrDefault(e => e.ID == id);
    }

    // ADD new employee
    public Employee Add(Employee employee)
    {
        employee.ID = _employees.Max(e => e.ID) + 1;
        _employees.Add(employee);

        // invalidate cache
        _cache.Remove(AllEmployeesKey);
        _cache.SetString(CacheKey(employee.ID), JsonSerializer.Serialize(employee));

        return employee;
    }

    // UPDATE employee
    public bool Update(int id, Employee updatedEmployee)
    {
        var existing = GetById(id);
        if (existing == null) return false;

        existing.Name = updatedEmployee.Name;
        existing.Salary = updatedEmployee.Salary;

        _cache.Remove(AllEmployeesKey);
        _cache.SetString(CacheKey(id), JsonSerializer.Serialize(existing));

        return true;
    }

    // DELETE employee
    public bool Delete(int id)
    {
        var employee = GetById(id);
        if (employee == null) return false;

        _employees.Remove(employee);
        _cache.Remove(CacheKey(id));
        _cache.Remove(AllEmployeesKey);

        return true;
    }

    private string CacheKey(int id) => $"Employee:{id}";

    private const string AllEmployeesKey = "Employee:all";
}
