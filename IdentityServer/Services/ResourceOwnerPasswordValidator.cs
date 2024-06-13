﻿using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var email = context.UserName;
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null && await _userManager.CheckPasswordAsync(user, context.Password))
        {
            if (!user.EmailConfirmed)
            {
                Dictionary<string, object> customData = new Dictionary<string, object>();
                customData.Add("user_id", user.Id);
                customData.Add("email", user.Email);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Email not confirmed", customData);
                return;
            }

            context.Result = new GrantValidationResult(
                subject: user.Id.ToString(),
                authenticationMethod: "pwd",
                claims: GetUserClaims(user));

            return;
        }

        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
    }

    private static IEnumerable<Claim> GetUserClaims(ApplicationUser user)
    {
        return new[]
        {
            new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
            new Claim(JwtClaimTypes.PreferredUserName, user.Email),
            new Claim(JwtClaimTypes.GivenName, user.FirstName)
        };
    }
}