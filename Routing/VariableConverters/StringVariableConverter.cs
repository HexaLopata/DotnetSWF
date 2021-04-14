namespace DotnetSWF.Routing.VariableConverters
{
    public class StringVariableConverter : IVariableConverter
    {
        public object Convert(string textRepresentation)
        {
            return textRepresentation;
        }
    }
}
