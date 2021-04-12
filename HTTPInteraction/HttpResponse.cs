using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DotnetSWF.HTTPInteraction
{
    public class HttpResponse : IHttpResponseResult
    {
        public Dictionary<string, string> Headers => _headers;
        public int ResponseCode => _responseCode;

        public static HttpResponse NotFound => new HttpResponse(new Dictionary<string, string>(), 404, "Error 404: Page or file not found");
        public static HttpResponse ServerError => new HttpResponse(new Dictionary<string, string>(), 500, "Error 500: Server error");
        public static HttpResponse OK => new HttpResponse(new Dictionary<string, string>(), 200, "");
        public byte[] Content { get => _content; set => _content = value; }

        private int _responseCode;
        private Dictionary<string, string> _headers;
        private byte[] _content;

        public HttpResponse(IDictionary<string, string> headers, int responseCode, string content)
        {
            _responseCode = responseCode;
            _headers = new Dictionary<string, string>(headers);
            Content = Encoding.UTF8.GetBytes(content);
        }

        public HttpResponse(IDictionary<string, string> headers, int responseCode, byte[] content)
        {
            _responseCode = responseCode;
            _headers = new Dictionary<string, string>(headers);
            Content = content;
        }

        public override string ToString()
        {
            string result = string.Empty;

            result += $"HTTP/1.1 {_responseCode}\n";
            foreach (var header in Headers)
            {
                result += header.Key + ": " + header.Value + "\n";
            }
            return result;
        }

        public void AppendString(string newContent)
        {
            string content = Encoding.UTF8.GetString(Content);
            content += newContent;
            Content = Encoding.UTF8.GetBytes(content);
        }

        public void AppendByteContent(byte[] bytes)
        {
            Content = Content.Concat(bytes).ToArray();
        }

        public byte[] GetBytes(Encoding encoding)
        {
            var result = encoding.GetBytes(ToString() + "\n");
            return result.Concat(Content).ToArray();
        }

        public HttpResponse GetHttpResponse()
        {
            return this;
        }
    }
}