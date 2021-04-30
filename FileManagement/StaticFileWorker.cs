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
                var filePath = CombineFilePathAndStaticFoulder(path);
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

        public string CombineFilePathAndStaticFoulder(string path)
        {
            string filePath = string.Empty;
            if (path.StartsWith("/"))
                filePath = _staticFileFolder + path;
            else
                filePath = _staticFileFolder + "/" + path;
            return filePath;
        }

        public string[] GetStringsFromFile(string path)
        {
            var filePath = CombineFilePathAndStaticFoulder(path);
            return File.ReadAllLines(filePath);
        }

        public string GetTextFromFile(string path)
        {
            var filePath = CombineFilePathAndStaticFoulder(path);
            return File.ReadAllText(filePath);
        }

        public bool TryGetFileAsBytes(string path, ref byte[] bytes)
        {
            try
            {
                var filePath = CombineFilePathAndStaticFoulder(path);
                bytes = GetFileAsBytes(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}