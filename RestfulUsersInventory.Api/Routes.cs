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
                name: typeof(UsersController).GetControllerName(),
                template: $"{ApiPrefix}/{{controller}}/{{action}}/{{id?}}",
                defaults: new
                {
                    controller = typeof(UsersController).GetControllerName(),
                    action = nameof(UsersController.GetUsers)
                }
            );

            routes.MapRoute
            (
                name: typeof(ItemsController).GetControllerName(),
                template: $"{ApiPrefix}/{{controller}}/{{action}}/{{id?}}",
                defaults: new
                {
                    controller = typeof(ItemsController).GetControllerName(),
                    action = nameof(ItemsController.GetItems)
                }
            );

            routes.MapRoute
            (
                name: typeof(UserInventoryController).GetControllerName(),
                template:
                    $"{ApiPrefix}/" +
                    $"{typeof(UsersController).GetControllerName()}/" +
                    "{userId}/{controller}/{action}/{itemId?}",
                defaults: new
                {
                    controller = typeof(UserInventoryController).GetControllerName(),
                    action = nameof(UserInventoryController.GetItemsForUser)
                }
            );
        }
    }
}
