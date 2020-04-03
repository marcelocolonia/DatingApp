using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DatingApp.API.DatingApp.Utils
{

    public static class Extensions
    {

        public static void AddApplicationError(this HttpResponse response, string message)
        {

            response.Headers.Add("Application-Error", message);
            response.Headers.Add(HeaderNames.AccessControlExposeHeaders, "Application-Error");
            response.Headers.Add(HeaderNames.AccessControlAllowOrigin, "*");

        }
    }

}
