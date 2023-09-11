namespace WebLibrary.HttpRequests
{
    public class GetRequest : HttpRequest
    {
        public GetRequest(string url, string controller) : base(url, controller)
        {
            HttpCommand = HttpCommand.Get;
        }

        public GetRequest(string url, string controller, params string[] parameters) : base(url, controller, parameters)
        {
            HttpCommand = HttpCommand.Get;
        }
    }
}
