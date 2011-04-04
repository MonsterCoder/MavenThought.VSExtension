using System;
using System.Windows.Input;
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

            var templatePath = ((Solution3)_dte.Solution).GetProjectItemTemplate("MaventThought.Test.Specification.Zip", "CSharp");
            e.targetProject.ProjectItems.AddFromTemplate(templatePath, e.SpecName);
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