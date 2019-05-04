using Microsoft.AspNetCore.Mvc;
using System;

namespace RestfulUsersInventory.Api.Extensions
{
    public static class TypeExtensions
    {
        public static string GetControllerName<T>(this T controllerType) where T : Type
        {
            if (!typeof(ControllerBase).IsAssignableFrom(controllerType))
            {
                return null;
            }

            string fullClassName = controllerType.Name;
            string controllerName = fullClassName.Replace(nameof(ControllerBase), string.Empty);
            return controllerName;
        }
    }
}
