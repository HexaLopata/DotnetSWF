using System.Reflection;
using DotnetSWF.HTTPInteraction;

namespace DotnetSWF.Routing
{
    public class RouteMethodInfo
    {
        public MethodInfo Method { get; set; }
        public Controller Source { get; set; }
        public HTTPRequestMethods HTTPMethod { get; private set;}

        public RouteMethodInfo(MethodInfo method, Controller source, HTTPRequestMethods hTTPMethod)
        {
            Method = method;
            Source = source;
            HTTPMethod = hTTPMethod;
        }
    }
}