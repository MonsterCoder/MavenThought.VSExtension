using EnvDTE;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Class for a single unit test
    /// </summary>
    public class TestItem : CodeItem
    {
        protected TestItem(string name, ProjectItem item) : base(name, item)
        {
        }
    }
}