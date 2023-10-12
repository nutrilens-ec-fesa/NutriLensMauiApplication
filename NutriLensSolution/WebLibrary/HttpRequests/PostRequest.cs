using Newtonsoft.Json;

namespace WebLibrary.HttpRequests
{
    public class PostRequest : HttpRequest
    {
        public object Body { get; set; }
        public string JsonBody { get => JsonConvert.SerializeObject(Body, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }); }

        public PostRequest(string url, string controller) : base(url, controller)
        {
            HttpCommand = HttpCommand.Post;
        }

        public PostRequest(string url, string controller, params string[] parameters) : base(url, controller, parameters)
        {
            HttpCommand = HttpCommand.Post;
        }
    }
}
