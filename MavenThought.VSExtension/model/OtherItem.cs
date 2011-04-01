using EnvDTE;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    public class OtherItem : CodeItem
    {
        public OtherItem(string name, ProjectItem item) : base(name, item)
        {
        }
    }
}