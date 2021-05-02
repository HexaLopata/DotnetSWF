using System.Collections.Generic;
using System;
using DotnetSWF.HTTPInteraction;
using DotnetSWF.FileManagement;
using DotnetSWF.Routing.VariableConverters;
using System.Reflection;

namespace DotnetSWF.Routing
{
    public class DefaultRouter : IRouter
    {
        private List<Controller> _controllers = new List<Controller>();
        private Dictionary<string, RouteMethodInfo> _pathsToTheMethods = new Dictionary<string, RouteMethodInfo>();
        private IStaticFileWorker _fileWorker;
        private Dictionary<Type, IVariableConverter> _converters = new Dictionary<Type, IVariableConverter>();

        public DefaultRouter(IStaticFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            InitializeConverters();
        }

        public void InitializeConverters()
        {
            _converters.Add(typeof(string), new StringVariableConverter());
            _converters.Add(typeof(int), new Int32VariableConverter());
            _converters.Add(typeof(bool), new BoolVariableConverter());
        }

        public IHttpResponseResult GetHttpResponseByRoute(HttpRequest request)
        {
            try
            {
                foreach (var path in _pathsToTheMethods)
                {
                    // Если запрошенный путь совпадает с путем метода
                    if (path.Key == request.Path)
                    {
                        List<object> args = new List<object>();

                        // Параметры подходящего метода
                        var parameters = path.Value.Method.GetParameters();
                        var requestVariables = request.Arguments;
                        args = GetConvertedAndSortedAccordedToMethodSignatureArguments(parameters, requestVariables);

                        try
                        {
                            if (path.Value.HTTPMethod == request.Method || path.Value.HTTPMethod == HTTPRequestMethods.ANY)
                            {
                                var source = path.Value.Source;
                                var method = path.Value.Method;
                                return (IHttpResponseResult)method.Invoke(source, args.ToArray());
                            }
                        }
                        catch (Exception ex)
                        {
                            HttpResponse result = HttpResponse.ServerError;
                            result.AppendString("\n" + ex.Message);
                            return result;
                        }
                    }
                }
                return _fileWorker.GetFileAsHttpResponse(request.Path);
            }
            catch (Exception ex)
            {
                HttpResponse result = HttpResponse.NotFound;
                result.AppendString("\n" + ex.Message);
                return result;
            }
        }

        public List<object> GetConvertedAndSortedAccordedToMethodSignatureArguments(ParameterInfo[] parameters, RequestArgumentInfo[] requestArguments)
        {
            List<object> resultList = new List<object>();
            foreach (var p in parameters)
            {
                bool isConverted = false;
                foreach (var arg in requestArguments)
                {
                    if (p.Name.ToUpper() == arg.Name.ToUpper())
                    {
                        try
                        {
                            resultList.Add(_converters[p.ParameterType].Convert(arg.Value));
                            isConverted = true;
                        }
                        catch (Exception)
                        {
                            throw new Exception("Argument has unknown type");
                        }
                    }
                }
                if (!isConverted)
                    throw new Exception("Invalid Arguments");
            }
            return resultList;
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
                    throw new Exception("Current path already exists");
                }
            }
        }
    }
}