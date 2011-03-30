namespace GeorgeChen.MavenThought_VSExtension
{
    public class ClassItem
    {
        public ClassItem(bool isTest, string name, string codeNamespace)
        {
            Name = name;
            CodeNamespace = codeNamespace;
            IsTestClass = isTest;
        }

        public string Name { get; private set; }

        public string CodeNamespace { get; set; }

        public bool IsTestClass { get; private set; }

        public bool IsTestSpecification { get; private set; }
    }
}