using System.Net;

namespace DatingApp.API.Utils
{
    public class PhotoDeleteResult
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
