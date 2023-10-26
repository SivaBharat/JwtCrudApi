using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JwtCrud.Models;
using Microsoft.AspNetCore.Authorization;

namespace JwtCrud.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase{ 
        private readonly JwtTokenContext _context;
        public EmployeesController(JwtTokenContext context){
            _context = context;
        }  
        
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(){
          if (_context.Employees == null){
              return NotFound();
          }
            return await _context.Employees.ToListAsync();
        }  
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id){
          if (_context.Employees == null){
              return NotFound();
          }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null){
                return NotFound();
            }
            return employee;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee){
            if (id != employee.EmpId){
                return BadRequest();
            }
            _context.Entry(employee).State = EntityState.Modified;
            try{
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException){
                if (!EmployeeExists(id)){
                    return NotFound();
                }
                else{
                    throw;
                }
            }
            return NoContent();
        }   
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee){
          if (_context.Employees == null){
              return Problem("Entity set 'JwtTokenContext.Employees'  is null.");
          }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEmployee", new { id = employee.EmpId }, employee);
        }     
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id){
            if (_context.Employees == null){
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null){
                return NotFound();
              }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool EmployeeExists(int id){
            return (_context.Employees?.Any(e => e.EmpId == id)).GetValueOrDefault();
        }
    }
}
