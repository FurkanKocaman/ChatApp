﻿using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ChatApp.Server.Application.Auth;

public sealed record LoginCommand(
    string UsernameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;
    //public string RefreshToken { get; set; }; 
}

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager, 
    SignInManager<AppUser> signInManager,
    IEmailService emailService,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(p => p.Email == request.UsernameOrEmail || p.UserName == request.UsernameOrEmail, cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("User not found");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if(signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
                return (500, $"Your user has been locked for {Math.Ceiling(timeSpan.Value.TotalMinutes)} minutes because you entered your password incorrectly 5 times.");
            else
                return (500, $"Your user has been locked for 5 minutes because you entered your password incorrectly 5 times.");
        }

        if (signInResult.IsNotAllowed)
        {
            string emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            string confirmationLink = $"https://localhost:7063/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(emailConfirmationToken)}";

            await emailService.SendAsync(user.Email!, "Email Confirmation Link", $"<p>E-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:</p><a href={confirmationLink}>E-posta Doğrula</a>", cancellationToken);

            return (500, "Confirmation link send to email");
        }
        if (!signInResult.Succeeded)
        {
            return (500, "Password is wrong");
        }

        //Token üret

        var token = await jwtProvider.CreateTokenAsync(user);

        var response = new LoginCommandResponse()
        {
            AccessToken = token
        };

        return response;
    }
}
