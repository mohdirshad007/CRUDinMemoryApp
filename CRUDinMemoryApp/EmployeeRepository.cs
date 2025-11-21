using CRUDinMemoryApp;

public class EmployeeRepository
{
    private static List<Employee> _employees = new List<Employee>()
    {
        new Employee { ID = 1, Name = "John Doe", Salary = 50000 },
        new Employee { ID = 2, Name = "Alice Smith", Salary = 60000 },
        new Employee { ID = 3, Name = "Bob Johnson", Salary = 55000 },
        new Employee { ID = 4, Name = "Carol White", Salary = 52000 },
        new Employee { ID = 5, Name = "David Brown", Salary = 65000 }
    };

    // GET all employees
    public IEnumerable<Employee> GetAll()
    {
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
        return employee;
    }

    // UPDATE employee
    public bool Update(int id, Employee updatedEmployee)
    {
        var employee = GetById(id);
        if (employee == null) return false;

        employee.Name = updatedEmployee.Name;
        employee.Salary = updatedEmployee.Salary;
        return true;
    }

    // DELETE employee
    public bool Delete(int id)
    {
        var employee = GetById(id);
        if (employee == null) return false;

        _employees.Remove(employee);
        return true;
    }
}
