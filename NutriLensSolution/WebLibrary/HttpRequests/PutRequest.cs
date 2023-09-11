using Newtonsoft.Json;

namespace WebLibrary.HttpRequests
{
    public class PutRequest : HttpRequest
    {
        public object Body { get; set; }
        public string JsonBody { get => JsonConvert.SerializeObject(Body); }

        public PutRequest(string url, string controller) : base(url, controller)
        {
            HttpCommand = HttpCommand.Put;
        }

        public PutRequest(string url, string controller, params string[] parameters) : base(url, controller, parameters)
        {
            HttpCommand = HttpCommand.Put;
        }
    }
}
