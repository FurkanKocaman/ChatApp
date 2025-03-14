using ChatApp.Server.Domain.Employees;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;

internal sealed class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRespository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
}


