using System;
using System.IO;
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

        public event EventHandler<SpecificationEventArgs> CreateSpecificationRequest = delegate { };

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
            if ((_dte.Solution==null)||(parameter == null) || !(parameter is CodeItem))
                {
                    return;
                }

                Open(parameter);
        }

        /// <summary>
        /// Open a class item
        /// </summary>
        /// <param name="target"></param>
        private void Open(ClassItem target)
        {
          if  (!NavigateTo(string.Format("{0}{1}{2}", target.Name , "Specification", Path.GetExtension(target.Item.Name))))
            
          {
              CreateSpecificationRequest(this, new SpecificationEventArgs(target));
          }
        }

        /// <summary>
        /// Open a specification item
        /// </summary>
        /// <param name="target"></param>
        private void Open(SpecificationItem target)
        {
            NavigateTo(string.Format("{0}{1}",target.Name.Replace("Specification", ""),  Path.GetExtension(target.Item.Name)));
        }        
        
        /// <summary>
        /// Open a senario item
        /// </summary>
        /// <param name="item"></param>
        private void Open(SenarioItem item)
        {
            Open(item.Specification);
        }

        private bool NavigateTo(string target)
        {
            var item = _dte.Solution.FindProjectItem(target);

            if (item == null)
            {
                return false;
            }

            var w = item.Open(Constants.vsViewKindCode);
            w.Visible = true;
            return true;
        }
    }
}
