using DotnetSWF.HTTPInteraction;

namespace DotnetSWF
{
    public interface IStaticFileWorker
    {
        string StaticFileFolder { get; set; }

        byte[] GetFileAsBytes(string path);
        string GetTextFromFile(string path);
        string[] GetStringsFromFile(string path);
        bool TryGetFileAsBytes(string path, ref byte[] bytes);
        IHttpResponseResult GetFileAsHttpResponse(string path);
    }
}