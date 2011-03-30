using System.Linq;
using EnvDTE;

namespace GeorgeChen.MavenThought_VSExtension
{
    internal static class ClassItemFactory
    {
        public static ClassItem Create(CodeClass classElement)
        {
            var isTest = DerivedFromBaseTest(classElement);
            return new ClassItem(isTest, classElement.Name, classElement.Namespace.Name);
        }

        private static bool DerivedFromBaseTest(CodeClass classElement)
        {
            if (classElement.Name == "BaseTest")
            {
                return true;
            }

            return classElement.Bases
                       .Cast<CodeElement>()
                       .Where(e => e.Kind == vsCMElement.vsCMElementClass)
                       .Count(c => DerivedFromBaseTest((CodeClass)c)) > 0;
        }
    }
}