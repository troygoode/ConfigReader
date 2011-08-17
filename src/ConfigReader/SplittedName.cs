namespace ConfigReader
{
    public class SplittedName
    {
        public readonly string TypeName;
        public readonly string Key;

        public SplittedName(string typeName, string key)
        {
            TypeName = typeName;
            Key = key;
        }
    }
}