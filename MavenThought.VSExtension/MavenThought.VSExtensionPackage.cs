using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using GeorgeChen.MavenThought_VSExtension.command;
using GeorgeChen.MavenThought_VSExtension.model;
using GeorgeChen.MavenThought_VSExtension.OptionDialog;
using Microsoft.VisualStudio.Shell;

namespace GeorgeChen.MavenThought_VSExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidMavenThought_VSExtensionPkgString)]
    [ProvideOptionPage(typeof(MavenThoughtOptions), "MavenThought", "Unit Test", 0, 0, true)]
    public sealed class MavenThought_VSExtensionPackage : Package
    {
        DTE2 dte;
        private NavigateCommand navigateCmd;


        /// <summary>
        /// Initialization of the package;
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if ( null != mcs )
            {
                // Create the command for the menu item.
                var navigationCommandID = new CommandID(GuidList.guidMavenThought_VSExtensionCmdSet, (int)PkgCmdIDList.cmdidTestNavigation);
                var navigationMenuItem = new MenuCommand(navigationMenuItemCallback, navigationCommandID);
                mcs.AddCommand(navigationMenuItem );
            }
        }

        /// <summary>
        /// </summary>
        private void navigationMenuItemCallback(object sender, EventArgs e)
        {
            if (this.dte == null)
            {
                this.dte = GetService(typeof(DTE)) as DTE2;
            }

            if (navigateCmd == null)
            {
                navigateCmd = new NavigateCommand(dte);
                navigateCmd.CreateSpecificationRequest += HandleCreateSpecificationRequest;
            }

            if (dte.ActiveDocument == null || dte.ActiveDocument.ProjectItem.FileCodeModel == null)
            {
                return;
            }

            var fileCodeModel = (FileCodeModel2)dte.ActiveDocument.ProjectItem.FileCodeModel;
            
            navigateCmd.Execute(ItemFactory.Create(fileCodeModel));
        }

        private void HandleCreateSpecificationRequest(object sender, SpecificationEventArgs e)
        {
          /// dte.Solution.FindProjectItem()
        }
    }
}