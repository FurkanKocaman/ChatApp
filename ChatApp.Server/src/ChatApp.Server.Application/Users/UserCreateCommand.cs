using ChatApp.Server.Application.Employees;
using ChatApp.Server.Domain.Users;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Users;

public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    DateTimeOffset BirtOfDate,
    bool? Gender,
    string Password) : IRequest<Result<string>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Firstname must be at least 3 characters");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Lastname must be at least 3 characters");
        RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters");
        RuleFor(x => x.Email).EmailAddress().WithMessage("This Email is not valid");
    }
}

internal sealed class UserCreateCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        bool isUsernameExist = await userManager.FindByNameAsync(request.Username) != null;
        if (isUsernameExist)
            return Result<string>.Failure("Username already exist");

        AppUser user = request.Adapt<AppUser>();
        var result = await userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
            return Result<string>.Failure("An error occured during creation.");
        return Result<string>.Succeed("User creation successfully completed");
    }
}
