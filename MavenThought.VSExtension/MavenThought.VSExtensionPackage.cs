using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace GeorgeChen.MavenThought_VSExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MyToolWindow))]
    [Guid(GuidList.guidMavenThought_VSExtensionPkgString)]
    public sealed class MavenThought_VSExtensionPackage : Package
    {
        DTE2 dte;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                var menuCommandID = new CommandID(GuidList.guidMavenThought_VSExtensionCmdSet, (int)PkgCmdIDList.cmdidMavenThought);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID );
                mcs.AddCommand( menuItem );
                // Create the command for the tool window
                var toolwndCommandID = new CommandID(GuidList.guidMavenThought_VSExtensionCmdSet, (int)PkgCmdIDList.cmdidMavenThoughtTool);
                var menuToolWin = new MenuCommand((a,e)=>ShowToolWindow(new MyControl()), toolwndCommandID);
                mcs.AddCommand( menuToolWin );
            }
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var classElements = new List<CodeElement2>();

            if (this.dte == null)
            {
                this.dte = GetService(typeof(DTE)) as DTE2; 
            }

            var fileCodeModel = (FileCodeModel2)dte.ActiveDocument.ProjectItem.FileCodeModel;

            CollectElements(classElements, fileCodeModel.CodeElements, new[] { vsCMElement.vsCMElementClass });

            var naviWindow = new TestListWindow { DataContext = classElements.Select(ce => ClassItemFactory.Create((CodeClass)ce)) };
            naviWindow.ShowDialog();
        }

        /// <summary>
        /// Collect code elements information
        /// </summary>
        /// <param name="list"></param>
        /// <param name="elements"></param>
        /// <param name="filter"></param>
        private void CollectElements(ICollection<CodeElement2> list, CodeElements elements, IEnumerable<vsCMElement> filter)
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

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the tool window. 
        /// </summary>
        private void ShowToolWindow(object content)
        {
            var window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }

            window.Content = content;
            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}