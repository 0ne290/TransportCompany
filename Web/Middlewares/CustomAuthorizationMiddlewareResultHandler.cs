using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Web.Middlewares;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        
        if (!authorizeResult.Succeeded)
        {
            var requiredRoles = policy.Requirements.OfType<RolesAuthorizationRequirement>();
            
            if (requiredRoles.Any(r => r.AllowedRoles.Contains("Administrator")))
                context.Response.Redirect("/login/administrator");
            else if (requiredRoles.Any(r => r.AllowedRoles.Contains("User")))
                context.Response.Redirect("/login/user");
        }
    }
    
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
}
