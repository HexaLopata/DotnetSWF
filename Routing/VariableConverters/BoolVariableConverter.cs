namespace DotnetSFW.Routing.VariableConverters
{
    public class BoolVariableConverter : IVariableConverter
    {
        public object Convert(string textRepresentation)
        {
            return bool.Parse(textRepresentation);
        }
    }
}
