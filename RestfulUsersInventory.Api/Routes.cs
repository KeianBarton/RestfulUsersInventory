using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using RestfulUsersInventory.Api.Controllers;
using RestfulUsersInventory.Api.Extensions;

namespace RestfulUsersInventory.Api
{
    public static class Routes
    {
        private const string ApiPrefix = "api";

        public static void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute
            (
                name: "default",
                template: $"{ApiPrefix}/{{controller}}/{{action}}/{{id?}}",
                defaults: new
                {
                    controller = typeof(UsersController).GetControllerName(),
                    action = nameof(UsersController.GetUsers)
                }
            );
        }
    }
}
