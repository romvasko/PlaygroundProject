using PlaygroundProject.ViewModels;

namespace PlaygroundProject.Controllers;


public class RouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        var matchingValue = values[routeKey]?.ToString();
        return Enum.TryParse(matchingValue, true, out Roles _);
    }
}
