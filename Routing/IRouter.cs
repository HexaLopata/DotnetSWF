using DotnetSWF.HTTPInteraction;

namespace DotnetSWF.Routing
{
    public interface IRouter
    {
        IHttpResponseResult GetHttpResponseByRoute(HttpRequest request);
        void RegisterController(Controller controller);
    }
}