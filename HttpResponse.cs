using System.Collections.Generic;
using System.Text;

namespace DotnetSWF
{
    public class HttpResponse
    {
        public Dictionary<string, string> Headers => _headers;
        public int ResponseCode => _responseCode;

        private int _responseCode;
        private Dictionary<string, string> _headers;

        public HttpResponse(IDictionary<string, string> headers, int responseCode)
        {
            _responseCode = responseCode;
            _headers = new Dictionary<string, string>(headers);
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

        public byte[] GetBytes(Encoding encoding)
        {
            return encoding.GetBytes(ToString());
        }
    }
}