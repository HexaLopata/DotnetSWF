namespace DotnetSWF
{
    public interface IRouter
    {
        IHttpResponseResult GetHttpResponseByRoute(HttpRequest request);
        void RegisterController(Controller controller);
    }
}