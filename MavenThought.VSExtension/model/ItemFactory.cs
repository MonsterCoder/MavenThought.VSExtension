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

        public static CodeItem Create(FileCodeModel2 codeModel)
        {
            var list = new List<CodeElement2>();
            CollectElements(list, codeModel.CodeElements, new [] { vsCMElement.vsCMElementClass });
            if (list.Count == 0)
            {
                return new OtherItem("Unknow", null);
            }

            var item = list.Cast<CodeClass>().First();

            if (item.IsDerivedFrom["MavenThought.Commons.Testing.BaseTest"])
            {
                if (item.Name.Contains("Specification"))
                {
                    return new SpecificationItem(item.Name, item.Namespace.Name, item.FullName, item.ProjectItem);  
                }

                var spec = (CodeClass)item.Bases.Cast<CodeElement>().First(ce => ce is CodeClass);
                var spItem = new SpecificationItem(spec.Name, spec.Namespace.Name, spec.FullName, item.ProjectItem);

                return new SenarioItem(item.Name, item.Namespace.Name, item.FullName, spItem, item.ProjectItem);
            }

            return new ClassItem(item.Name, item.Namespace.Name, item.FullName, item.ProjectItem);
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