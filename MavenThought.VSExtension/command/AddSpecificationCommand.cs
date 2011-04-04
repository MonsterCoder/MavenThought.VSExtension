using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;

namespace GeorgeChen.MavenThought_VSExtension.command
{
    public class AddSpecificationCommand : ICommand
    {
        private readonly DTE2 _dte;

        public AddSpecificationCommand(DTE2 dte)
        {
            _dte = dte;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            if (parameter == null || !(parameter is SpecificationEventArgs || ((SpecificationEventArgs)parameter).targetProject == null))
            {
                return;
            }

            var e = parameter as SpecificationEventArgs;
            var path = new List<ProjectItem>();

            var projectItems = e.Source.Item.ContainingProject.ProjectItems;
            VisitPath(path, e.Source.Item.Name, projectItems);

            var templatePath = ((Solution3)_dte.Solution).GetProjectItemTemplate("MaventThought.Test.Specification.Zip", "CSharp");

            if (path.Count == 0)
            {
                e.targetProject.ProjectItems.AddFromTemplate(templatePath, e.SpecName);
            }
            else
            {
                path.Reverse();
                ProjectItem current = null;

                foreach (var folder in path)
                {
                    if (current == null)
                    {
                        current = e.targetProject.ProjectItems.Cast<ProjectItem>().FirstOrDefault(p => p.Name == folder.Name) ??
                               e.targetProject.ProjectItems.AddFolder(folder.Name);
                    }
                    else
                    {

                        current = current.ProjectItems.Cast<ProjectItem>().FirstOrDefault(p => p.Name == folder.Name) ??
                                   current.ProjectItems.AddFolder(folder.Name); 
                    }
                }

                current.ProjectItems.AddFromTemplate(templatePath, e.SpecName);
            }
        }

        private bool VisitPath(ICollection<ProjectItem> path, string itemname, ProjectItems projectItems)
        {
            if (projectItems == null || projectItems.Count == 0)
            {
                return false;
            }

            if (projectItems.Cast<ProjectItem>().Any(i => i.Name == itemname))
            {
                return !(projectItems.Parent is Project);
            }

            foreach (ProjectItem projectItem in projectItems)
            {
                if  (VisitPath(path, itemname, projectItem.ProjectItems))
                {
                    path.Add(projectItem);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}