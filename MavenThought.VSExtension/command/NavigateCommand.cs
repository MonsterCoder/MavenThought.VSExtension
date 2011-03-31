using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Input;
using EnvDTE;
using GeorgeChen.MavenThought_VSExtension.model;

namespace GeorgeChen.MavenThought_VSExtension.command
{
    public class NavigateCommand : ICommand
    {
        private readonly IEnumerable<Project> _projects;

        public NavigateCommand(IEnumerable<Project> projects)
        {
            _projects = projects;
        }

        public event EventHandler CanExecuteChanged = delegate { };

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

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(dynamic  parameter)
        {
                if ((parameter == null) || !(parameter is CodeItem))
                {
                    return;
                }

                Open(parameter);
        }

        /// <summary>
        /// Open a class item
        /// </summary>
        /// <param name="item"></param>
        private void Open(ClassItem item)
        {
            NavigateTo(item.Name + "Specification");
        }

        /// <summary>
        /// Open a specification item
        /// </summary>
        /// <param name="item"></param>
        private void Open(SpecificationItem item)
        {
            NavigateTo(item.Name.Replace("Specification", ""));
        }        
        
        /// <summary>
        /// Open a senario item
        /// </summary>
        /// <param name="item"></param>
        private void Open(SenarioItem item)
        {
            Open(item.Specification);
        }

        private void NavigateTo(string target)
        {
            _projects.Any(project => SearchAndOpen(project.ProjectItems, target));
        }

        private bool SearchAndOpen(ProjectItems projectItems, string target)
        {
            if (projectItems == null)
            {
                return false;
            }

            foreach (ProjectItem projectItem in projectItems)
            {

                if (projectItem.FileCodeModel != null)
                {
                    if (Path.GetFileNameWithoutExtension(projectItem.Name) == target)
                    {
                        var w = projectItem.Open(Constants.vsViewKindCode);
                        w.Visible = true;
                        return true;
                    }
                }
                else if (projectItem.SubProject != null)
                {
                    if (SearchAndOpen(projectItem.SubProject.ProjectItems, target))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
