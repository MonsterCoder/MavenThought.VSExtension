using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows;
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
            var testProjectname = string.Format("{0}.Tests", e.Source.Item.ContainingProject.Name);
            var targetProject = FindProject(dte.Solution.Projects.Cast<Project>(), testProjectname);

            if (targetProject==null)
            {
                MessageBox.Show(string.Format("Do you want to create test project {0}?", testProjectname));
                //var specitem = targetProject.ProjectItems.AddFromTemplate(e.SpecName, e.SpecName);
                //var w = specitem.Open(Constants.vsViewKindCode);
                //w.Visible = true;
            }
            else
            {
               /// targetProject.ProjectItems.AddFromTemplate("CSharpProjects", "CSharpProjects");
            }
        }

        private Project FindProject(IEnumerable<Project> projects, string testProjectname)
        {
            if (projects == null || projects.Count() == 0)
            {
                return null;
            }

            var target = projects.FirstOrDefault(p => p.Name.Equals(testProjectname, StringComparison.InvariantCultureIgnoreCase) && p.Kind != ProjectKinds.vsProjectKindSolutionFolder);

           if (target!= null)
           {
               return target;
           }

           var solutionfoleders = projects.Where(p => p.Kind == ProjectKinds.vsProjectKindSolutionFolder);

           return FindProject(solutionfoleders.Where(folder => folder.ProjectItems != null).SelectMany(folder => folder.ProjectItems.Cast<ProjectItem>()), testProjectname);
        }

        private Project FindProject(IEnumerable<ProjectItem> items, string testProjectname)
        {
            if (items == null || items.Count() == 0)
            {
                return null;
            }

           var target = items.FirstOrDefault(item => (item.Name.Equals(testProjectname, StringComparison.InvariantCultureIgnoreCase)));

           if (target != null)
           {
               return target.Object as Project;
           }

           return FindProject(items.Where(item => item.SubProject !=null).Select(item => item.SubProject), testProjectname);
        }
    }
}