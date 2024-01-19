using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SneakerShoeStoreClient.DTO.Login;

namespace SneakerShoeStoreClient.Authorization
{
    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;
            var serializeMember = session.GetString("loginUser");
            if (serializeMember != null)
            {
                var member = JsonConvert.DeserializeObject<UserModel>(serializeMember);
                if (member.Role.Equals("admin"))
                {
                    return;
                }
            }
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
