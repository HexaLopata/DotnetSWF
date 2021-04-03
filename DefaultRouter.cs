using System.Collections.Generic;
using System;

namespace DotnetSWF
{
    public class DefaultRouter : IRouter
    {
        private List<Controller> _controllers = new List<Controller>();
        private Dictionary<string, RouteMethodInfo> _pathsToTheMethods = new Dictionary<string, RouteMethodInfo>();

        public IHttpResponseResult GetHttpResponseByRoute(HttpRequest request)
        {
            try
            {
                foreach (var path in _pathsToTheMethods)
                {
                    if (path.Key == request.Path)
                    {
                        var source = path.Value.Source;
                        var controller = path.Value.Method;
                        return (IHttpResponseResult)controller.Invoke(source, new object[] { });
                    }
                }
                // Сделать работника со статическими файлами
                return HttpResponse.NotFound;
            }
            catch(Exception ex)
            {
                HttpResponse result = HttpResponse.ServerError;
#if Debug
                result.Content += "\n" + ex.Message;
#endif
                return result;
            }
        }

        public void RegisterController(Controller controller)
        {
            var methods = controller.RouteMethods;
            foreach (var method in methods)
            {
                try
                {
                    _pathsToTheMethods.Add(method.Key, method.Value);
                }
                catch (ArgumentException)
                {
                    throw new Exception("There are 2 or more methods with the same paths");
                }
            }
        }
    }
}