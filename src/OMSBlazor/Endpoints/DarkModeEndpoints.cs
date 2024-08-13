using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OMSBlazor.Services;

namespace OMSBlazor.Endpoints
{
    public static class DarkModeEndpoints
    {
        public static void MapDarkModeEndpoints(this WebApplication app)
        {
            app.MapGet("api/darkMode", GetHandler);
            app.MapPost("api/darkMode", PostHandler);
        }

        private static IResult GetHandler(HttpContext context)
        {
            var isDarkMode = DarkModeState.GetIsDarkMode();
            return Results.Ok(isDarkMode);
        }

        private static IResult PostHandler(HttpContext context, IsDarkModeJson isDarkModeJson)
        {
            DarkModeState.SetIsDarkMode(isDarkModeJson.IsDarkMode);
            return Results.Ok();
        }
    }

    public class IsDarkModeJson
    {
        public bool IsDarkMode { get; set; }
    }
}
