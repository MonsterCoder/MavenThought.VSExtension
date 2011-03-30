using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using EnvDTE;
using EnvDTE80;
using GeorgeChen.MavenThought_VSExtension.model;

namespace GeorgeChen.MavenThought_VSExtension.command
{
    public class NavigateCommand : ICommand
    {
        private readonly DTE2 _dte;

        public NavigateCommand(DTE2 dte)
        {
            _dte = dte;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
                if ((parameter == null) || !(parameter is FileCodeModel2))
                {
                    return;
                }

                dynamic codeItem = ItemFactory.Create((FileCodeModel2)parameter);

                Open(codeItem);
        }

        /// <summary>
        /// Open a class item
        /// </summary>
        /// <param name="item"></param>
        private void Open(ClassItem item)
        {
            MessageBox.Show("ClassItem");
        }        
        
        /// <summary>
        /// Open a specification item
        /// </summary>
        /// <param name="item"></param>
        private void Open(SpecificationItem item)
        {
            var classname = item.Name.Replace("Specification", "");
            foreach (Project project in _dte.Solution.Projects)
            {
                foreach (ProjectItem projectItem in project.ProjectItems)
                {
                    if (projectItem.FileCodeModel != null)
                    {
                        if (Path.GetFileNameWithoutExtension(projectItem.Name) == classname)
                        {
                           var w = projectItem.Open(Constants.vsViewKindCode);
                            w.Visible = true;
                        }
                    }
                }
            }   
        }        
        
        /// <summary>
        /// Open a senario item
        /// </summary>
        /// <param name="item"></param>
        private void Open(SenarioItem item)
        {
            MessageBox.Show("SenarioItem");
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



    }
}