using ApplicationLayer.Contracts;
using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Implementations
{
    public class EmployeeRepo(AppDbContext appDbContext) : IEmployee
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResponse> AddAsync(Employee employee)
        {
            var check = await _appDbContext.Employees
                .FirstOrDefaultAsync(x => x.Name!.ToLower() == employee.Name!.ToLower());

            if (check is not null)
                return new ServiceResponse(false, "User already exist");

            _appDbContext.Employees.Add(employee);
            await SaveChangesAsync();

            return new ServiceResponse(true, "User has been successfully added");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var employee = await _appDbContext.Employees.FindAsync(id);

            if (employee is null)
                return new ServiceResponse(false, "User was not found");

            _appDbContext.Employees.Remove(employee);
            await SaveChangesAsync();

            return new ServiceResponse(true, "User has been successfully deleted");
        }

        public async Task<List<Employee>> GetAsync() => 
            await _appDbContext.Employees.AsNoTracking().ToListAsync();

        public async Task<Employee> GetByIdAsync(int id) => 
            await _appDbContext.Employees.FindAsync(id);

        public async Task<ServiceResponse> UpdateAsync(Employee employee)
        {
            _appDbContext.Update(employee);
            await SaveChangesAsync();

            return new ServiceResponse(true, "User has been successfully updated");
        }

        private async Task SaveChangesAsync() => await _appDbContext.SaveChangesAsync();
    }
}
