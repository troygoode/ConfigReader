using System;
using System.Text.RegularExpressions;

namespace ConfigReader
{
    public class NameSplitter
    {
        private const string typeCapture = "Type";
        private const string keyCapture = "Key";
        private static readonly Regex splitter = new Regex(String.Format(@"(?<{0}>\w+)\.(?<{1}>\w+)", typeCapture, keyCapture), RegexOptions.Compiled);

        public SplittedName Split(string name)
        {
            if (name == null || !splitter.IsMatch(name))
                return null;

            var groups = splitter.Match(name).Groups;
           
            string typeName = groups[typeCapture].Captures[0].Value;
            string keyName = groups[keyCapture].Captures[0].Value;

            return new SplittedName(typeName, keyName);
        }
    }
}