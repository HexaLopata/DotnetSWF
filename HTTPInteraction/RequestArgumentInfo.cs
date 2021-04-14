namespace DotnetSFW.HTTPInteraction
{
    public class RequestArgumentInfo
    {
        private string _name;
        private string _value;

        public string Name => _name;
        public string Value => _value;

        public RequestArgumentInfo(string name, string value)
        {
            _name = name;
            _value = value;
        }
    }
}
