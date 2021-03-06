using System.IO;
using DotnetSWF.HTTPInteraction;

namespace DotnetSWF.FileManagement
{
    public class StaticFileWorker : IStaticFileWorker
    {
        private string _staticFileFolder = "Static";

        public string StaticFileFolder
        {
            get => _staticFileFolder;
            set => _staticFileFolder = value;
        }

        public byte[] GetFileAsBytes(string path)
        {
            return File.ReadAllBytes(_staticFileFolder + path);
        }

        public IHttpResponseResult GetFileAsHttpResponse(string path)
        {
            try
            {
                string filePath = string.Empty;
                if(path.StartsWith("/"))
                    filePath = _staticFileFolder + path;
                else
                    filePath = _staticFileFolder + "/" + path;
                var file = File.ReadAllBytes(filePath);
                HttpResponse result = HttpResponse.OK;
                result.AppendByteContent(file);
                return result;   
            }
            catch
            {
                HttpResponse notFound = HttpResponse.NotFound;
                notFound.AppendString(" Static file path: " + path);
                return notFound;
            }
        }

        public string[] GetStringsFromFile(string path)
        {
            return File.ReadAllLines(_staticFileFolder + path);
        }

        public string GetTextFromFile(string path)
        {
            return File.ReadAllText(_staticFileFolder + path);
        }

        public bool TryGetFileAsBytes(string path, ref byte[] bytes)
        {
            try
            {
                bytes = GetFileAsBytes(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}