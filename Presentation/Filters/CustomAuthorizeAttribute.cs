using _2_Domain.IAM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _1_API.Filters;

public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;
    
    public CustomAuthorizeAttribute(params string[] roles)
    {
        this._roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as UserInformation;
        
        if (this._roles.Any() && !_roles.Contains(user.Role))
        {
            context.Result = new ForbidResult();
        }
    }
}