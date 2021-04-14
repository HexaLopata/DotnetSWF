using System.Collections.Generic;
using System.Text;
using System.Net;
using System;

namespace DotnetSWF.HTTPInteraction
{
    public enum HTTPRequestMethods
    {
        GET,
        POST,
        PUT,
        DELETE,
        ANY
    }

    public class HttpRequest
    {
        public Dictionary<string, string> Headers => _headers;
        public HTTPRequestMethods Method => _method;
        public string Path => _path;
        public RequestArgumentInfo[] Arguments => _args;

        private Dictionary<string, string> _headers;
        private HTTPRequestMethods _method;
        private string _path;
        private RequestArgumentInfo[] _args;

        public HttpRequest(IDictionary<string, string> headers, HTTPRequestMethods method, string path)
        {
            _headers = new Dictionary<string, string>(headers);
            _method = method;
            _args = ParseArguments(path);
            if(path.Contains('?'))
            {
                _path = path.Substring(0, path.IndexOf('?'));
            }
            else
                _path = path;
        }

        public override string ToString()
        {
            string result = string.Empty;

            result += $"{_method} {_path} HTTP/1.1\n";
            foreach (var header in Headers)
            {
                result += header.Key + ": " + header.Value + "\n";
            }
            return result;
        }

        public static bool TryParse(string requestString, ref HttpRequest request)
        {
            try
            {
                request = Parse(requestString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static HttpRequest Parse(string requestString)
        {
            try
            {
                string[] requestStringAsArray = requestString.Split(' ');
                string method = requestStringAsArray[0];
                string path = requestStringAsArray[1];
                string[] requestStrings = requestString.Split('\n');
                Dictionary<string, string> headers = new Dictionary<string, string>();
                for (int i = 1; i < requestStrings.Length; i++)
                {
                    string headerName = requestStrings[i].Split(':')[0];
                    if (IsHeaderExists(headerName))
                    {
                        var index = requestStrings[i].IndexOf(':');
                        var headerValue = requestStrings[i].Substring(index + 2);
                        headers.Add(headerName, headerValue);
                    }
                }

                if (IsMethodExists(method))
                {
                    return new HttpRequest(headers, Enum.Parse<HTTPRequestMethods>(method.ToUpper()), path);
                }
                throw new Exception("HttpRequest Parse Error");

            }
            catch (Exception)
            {
                throw new Exception("HttpRequest Parse Error");
            }
        }

        private static RequestArgumentInfo[] ParseArguments(string path)
        {
            List<RequestArgumentInfo> resultList = new List<RequestArgumentInfo>();
            if (path.Contains('?'))
            {
                try
                {
                    var argsString = path.Substring(path.IndexOf('?') + 1);
                    var args = argsString.Split('&');
                    foreach(var a in args)
                    {
                        var nameAndValue = a.Split('=');
                        resultList.Add(new RequestArgumentInfo(nameAndValue[0], nameAndValue[1]));
                    }
                    return resultList.ToArray();
                }
                catch
                {
                    return resultList.ToArray();
                }
            }
            return resultList.ToArray();
        }

        public byte[] GetBytes(Encoding encoding)
        {
            return encoding.GetBytes(ToString());
        }

        private static bool IsMethodExists(string method)
        {
            string[] methods = Enum.GetNames(typeof(HTTPRequestMethods));
            foreach (var m in methods)
            {
                if (method.ToUpper() == m.ToUpper())
                    return true;
            }
            return false;
        }

        private static bool IsHeaderExists(string header)
        {
            string[] possibleHeaders = Enum.GetNames(typeof(HttpRequestHeader));
            foreach (var h in possibleHeaders)
            {
                if (header.Replace("-", "").ToUpper() == h.ToUpper())
                    return true;
            }
            return false;
        }
    }
}