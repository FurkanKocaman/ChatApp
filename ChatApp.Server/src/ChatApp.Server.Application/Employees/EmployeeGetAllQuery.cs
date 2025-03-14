using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Employees;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Employees;

public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

public sealed class EmployeeGetAllQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly BirtOfDate { get; set; }
    public decimal Salary { get; set; }
    public string TCNo { get; set; } = default!;
}

internal sealed class EmployeeGetAllQueryhandler(
    IEmployeeRespository employeeRespository,
    UserManager<AppUser> userManager) : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
{
    public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from employee in employeeRespository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on employee.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on employee.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new EmployeeGetAllQueryResponse
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Salary = employee.Salary,
                            BirtOfDate = employee.BirtOfDate,
                            CreatedAt = employee.CreatedAt,
                            DeleteAt = employee.DeleteAt,
                            Id = employee.Id,
                            IsDeleted = employee.IsDeleted,
                            TCNo = employee.PersonalInformation.TCNo,
                            UpdateAt = employee.UpdateAt,
                            CreateUserId = employee.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateUserId = employee.UpdateUserId,
                            UpdateUserName = employee.UpdateUserId == null ? null : update_users.FirstName +" " + update_users.LastName +" ("+
                            update_users.Email + ")",
                        });
            
            
            
            
            employeeRespository.GetAll()
             .Select(s => new EmployeeGetAllQueryResponse
             {
                 FirstName = s.FirstName,
                 LastName = s.LastName,
                 Salary = s.Salary,
                 BirtOfDate = s.BirtOfDate,
                 CreatedAt = s.CreatedAt,
                 DeleteAt = s.DeleteAt,
                 Id = s.Id,
                 IsDeleted = s.IsDeleted,
                 TCNo = s.PersonalInformation.TCNo,
                 UpdateAt = s.UpdateAt,
             }).AsQueryable();

        return Task.FromResult(response);
    }
}
