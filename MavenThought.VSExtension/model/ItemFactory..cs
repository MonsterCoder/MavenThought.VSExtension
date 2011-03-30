using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    internal static class ItemFactory
    {
        public static ClassItem Create(CodeClass classElement)
        {
            return null;
            //var isTest = DerivedFromBaseTest(classElement);
            //return new ClassItem(isTest, classElement.Name, classElement.Namespace.Name);
        }

        public static CodeItem Create(FileCodeModel2 codeModel)
        {
            var list = new List<CodeElement2>();
            CollectElements(list, codeModel.CodeElements, new [] { vsCMElement.vsCMElementClass });
            if (list.Count == 0)
            {
                return new OtherItem("Unknow");
            }

            var item = list.Cast<CodeClass>().First();

            if (item.IsDerivedFrom["MavenThought.Commons.Testing.BaseTest"])
            {
                if (item.Name.Contains("Specification"))
                {
                    return new SpecificationItem(item.Name, item.Namespace.Name, item.FullName);  
                }

                return new SenarioItem(item.Name, item.Namespace.Name, item.FullName);
            }

            return new ClassItem(item.Name, item.Namespace.Name, item.FullName);
        }


        /// <summary>
        /// Collect code elements information
        /// </summary>
        /// <param name="list"></param>
        /// <param name="elements"></param>
        /// <param name="filter"></param>
        public static void CollectElements(ICollection<CodeElement2> list, CodeElements elements, IEnumerable<vsCMElement> filter)
        {
            if (elements == null || elements.Count == 0)
            {
                return;
            }

            foreach (CodeElement2 p in elements)
            {
                Trace.WriteLine(String.Format("+ {0}", p.Kind));
                if (filter.Contains(p.Kind))
                {
                    list.Add(p);
                }

                CollectElements(list, p.Children, filter);
            }
        }
    }
}