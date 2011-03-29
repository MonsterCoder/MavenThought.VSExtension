using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
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
        DTE dte;

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. 
        /// </summary>
        private void ShowToolWindow(object content)
        {
            var window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            window.Content = "ddddd";
            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

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
            if (this.dte == null)
            {
                this.dte = GetService(typeof(DTE)) as DTE; 
            }
            var projects = new List<string>();

            foreach (dynamic p in dte.Solution.Projects)
            {
                projects.Add(p.Name);
            }

            var naviWindow = new TestListWindow { DataContext = projects };
            naviWindow.ShowDialog();
        }
    }
}