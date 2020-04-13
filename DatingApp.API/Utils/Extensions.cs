using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DatingApp.API.Utils
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add(HeaderNames.AccessControlExposeHeaders, "Application-Error");
            response.Headers.Add(HeaderNames.AccessControlAllowOrigin, "*");
        }

        public static int CalculateAge(this DateTime datetime)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);

            TimeSpan span = DateTime.UtcNow - datetime;
            // Because we start at year 1 for the Gregorian
            // calendar, we must subtract a year here.
            return (zeroTime + span).Year - 1;
        }

        public static int LoggedUserId(this HttpContext context)
        {
            return int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
