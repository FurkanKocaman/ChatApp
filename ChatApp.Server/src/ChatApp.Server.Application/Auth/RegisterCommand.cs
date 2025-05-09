﻿using ChatApp.Server.Domain.Users;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Auth;
public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    DateTimeOffset BirthOfDate,
    bool? Gender
    ) : IRequest<Result<LoginCommandResponse>>;

internal sealed class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    ISender sender
    ) : IRequestHandler<RegisterCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userExist = await userManager.FindByEmailAsync(request.Email);

        if (userExist is not null)
            return Result<LoginCommandResponse>.Failure("User with this email is exist");

        AppUser user = request.Adapt<AppUser>();
        user.CreatedAt = DateTimeOffset.Now;
        user.CreateUserId = user.Id;

        IdentityResult result = await userManager.CreateAsync(user,request.Password);

        if (!result.Succeeded)
            return Result<LoginCommandResponse>.Failure("Error when user creation "+ result.Errors);

        LoginCommand loginCommand = new(request.UserName, request.Password);
        var loginResult = await sender.Send(loginCommand,cancellationToken);

        return loginResult;
    }
}
