using System;
using DotnetSWF.HTTPInteraction;

namespace DotnetSWF.Routing
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteMethodAttribute : Attribute
    {
        private string _path;
        private HTTPRequestMethods _hTTPMethod;

        public string Path => _path;
        public HTTPRequestMethods HTTPRequestMethod => _hTTPMethod;

        public RouteMethodAttribute(string path, HTTPRequestMethods hTTPMethod = HTTPRequestMethods.ANY)
        {
            _hTTPMethod = hTTPMethod;
            _path = path;
        }
    }
}