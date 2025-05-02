using ChatApp.Server.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Infrastructure.Configurations;
public sealed class AppRoleValidator : RoleValidator<AppRole>
{
    public override async Task<IdentityResult> ValidateAsync(RoleManager<AppRole> roleManager, AppRole appRole)
    {
        var roleName = await roleManager.GetRoleNameAsync(appRole);

        if (string.IsNullOrWhiteSpace(roleName))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "RoleNameIsNotValid",
                Description = "Role Name is not valid!"
            });
        }
        else
        {
            var owner = await roleManager.Roles.FirstOrDefaultAsync(x => x.ServerId == appRole.ServerId && x.NormalizedName == appRole.NormalizedName);

            if (owner is not null && string.Equals(roleManager.GetRoleIdAsync(owner), roleManager.GetRoleIdAsync(appRole))){
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateRoleName",
                    Description = "this role already exist in this App!"
                });
            }
        }
        return IdentityResult.Success;
    }
}
