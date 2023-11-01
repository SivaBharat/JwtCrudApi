using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JwtCrud.CustomAuthorize
{
    public class BlockAttribute : TypeFilterAttribute
    {
        public BlockAttribute() : base(typeof(BlockFilter))
        {
        }
    }

    public class BlockFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);

            }

        }
    }
}
