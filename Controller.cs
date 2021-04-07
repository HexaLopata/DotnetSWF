using System.Collections.Generic;
using System.Reflection;
using System;
using DotnetSWF.Routing;
using DotnetSWF.HTTPInteraction;

namespace DotnetSWF
{
    public abstract class Controller
    {
        public Dictionary<string, RouteMethodInfo> RouteMethods => _routeMethods;

        protected IStaticFileWorker _fileWorker = new StaticFileWorker();

        private Dictionary<string, RouteMethodInfo> _routeMethods = new Dictionary<string, RouteMethodInfo>();

        public Controller()
        {
            var methods = this.GetType().GetMethods();
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes();
                if (method.ReturnType == typeof(IHttpResponseResult))
                {
                    foreach (var attribute in attributes)
                    {
                        RouteMethodAttribute routeMethodAttribute;
                        if ((routeMethodAttribute = attribute as RouteMethodAttribute) != null)
                        {
                            try
                            {
                                var routeMethodInfo = new RouteMethodInfo(method, this, routeMethodAttribute.HTTPRequestMethod);
                                _routeMethods.Add(routeMethodAttribute.Path, routeMethodInfo);
                            }
                            catch (ArgumentException)
                            {
                                throw new Exception("There are 2 or more methods with the same paths");
                            }
                        }
                    }
                }
            }
        }
    }
}