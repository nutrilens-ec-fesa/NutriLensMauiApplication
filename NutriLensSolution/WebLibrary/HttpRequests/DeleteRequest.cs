namespace WebLibrary.HttpRequests
{
    public class DeleteRequest : HttpRequest
    {
        public DeleteRequest(string url, string controller) : base(url, controller)
        {
            HttpCommand = HttpCommand.Delete;
        }

        public DeleteRequest(string url, string controller, params string[] parameters) : base(url, controller, parameters)
        {
            HttpCommand = HttpCommand.Delete;
        }
    }
}
