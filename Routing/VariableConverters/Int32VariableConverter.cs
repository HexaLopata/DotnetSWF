namespace DotnetSWF.Routing.VariableConverters
{
    public class Int32VariableConverter : IVariableConverter
    {
        public object Convert(string textRepresentation)
        {
            return int.Parse(textRepresentation);
        }
    }
}
