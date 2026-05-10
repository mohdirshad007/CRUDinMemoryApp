using CRUDinMemoryApp;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repo;

    // change1
    public EmployeeController(EmployeeRepository repo)
    {
        _repo = repo;
    }

    // GET /Employee
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repo.GetAll());
    }

    // GET /Employee/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var emp = _repo.GetById(id);
        if (emp == null) return NotFound($"Employee with ID {id} not found");
        return Ok(emp);
    }

    // POST /Employee
    [HttpPost]
    public IActionResult Add(Employee employee)
    {
        var newEmp = _repo.Add(employee);
        return CreatedAtAction(nameof(GetById), new { id = newEmp.ID }, newEmp);
    }

    // PUT /Employee/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, Employee employee)
    {
        var updated = _repo.Update(id, employee);
        if (!updated) return NotFound($"Employee with ID {id} not found");

        return Ok($"Employee {id} updated successfully");
    }

    // DELETE /Employee/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _repo.Delete(id);
        if (!deleted) return NotFound($"Employee with ID {id} not found");

        return Ok($"Employee {id} deleted successfully");
    }
}
