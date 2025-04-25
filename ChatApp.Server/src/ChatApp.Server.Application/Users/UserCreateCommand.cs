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
    string UserName,
    string Email,
    DateTimeOffset? BirtOfDate,
    string? AvatarUrl,
    bool? Gender,
    string Password) : IRequest<Result<string>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Firstname must be at least 3 characters");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Lastname must be at least 3 characters");
        RuleFor(x => x.UserName).MinimumLength(3).WithMessage("Username must be at least 3 characters");
        RuleFor(x => x.Email).EmailAddress().WithMessage("This Email is not valid");
    }
}

internal sealed class UserCreateCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        bool isUsernameExist = await userManager.FindByNameAsync(request.UserName) != null;
        if (isUsernameExist)
            return Result<string>.Failure("Username already exist");

        AppUser user = request.Adapt<AppUser>();
        user.BirthOfDate = request.BirtOfDate;
        user.CreatedAt = DateTimeOffset.Now;
        user.CreateUserId = user.Id;
        var result = await userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
            return Result<string>.Failure("An error occured during creation.");
        return Result<string>.Succeed("User creation successfully completed");
    }
}
