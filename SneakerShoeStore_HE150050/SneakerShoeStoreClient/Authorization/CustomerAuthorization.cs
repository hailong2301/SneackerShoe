using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SneakerShoeStoreClient.Authorization
{
    public class CustomerAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;
            var serializeMember = session.GetString("loginUser");
            if (serializeMember != null)
            {
                return;
            }
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
