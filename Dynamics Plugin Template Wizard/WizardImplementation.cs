using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Shared_Tools;
using System;
using System.Collections.Generic;
using NuGet.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace Dynamics_Plugin_Template
{
    public class WizardImplementation : IWizard
    {
        // This method is called before opening any item that   
        // has the OpenInEditor attribute.  
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            project.DTE.StatusBar.Text = "Generating Strong Name Key";
            StrongNameGenerator.GenerateKey(project);
            project.DTE.StatusBar.Text = "Restoring NuGet Packages";
            var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            IVsPackageInstaller2 packageInstaller = componentModel.GetService<IVsPackageInstaller2>();

            IDictionary<string, string> packageVersions = new Dictionary<string, string>();
            packageVersions.Add("ILMerge", "3.0.41");
            packageVersions.Add("ILMerge.MSBuild.Task", "1.0.7");
            foreach (Property item in project.Properties)
            {
                if (item.Name == "TargetFrameworkMoniker" && (item.Value + "").StartsWith(".NETFramework,Version=v"))
                {
                    ;
                    if (Version.Parse((item.Value + "").Substring(".NETFramework,Version=v".Length)) >= Version.Parse("4.6.2"))
                    {
                        packageVersions.Add("Microsoft.CrmSdk.CoreAssemblies", "9.0.2.26");
                    }
                    else
                    {
                        packageVersions.Add("Microsoft.CrmSdk.CoreAssemblies", "9.0.2.5"); //ToDo: Fix Bug with v4.5.2
                    }
                    break;
                }
            }

            packageInstaller.InstallPackagesFromVSExtensionRepository(
                extensionId: "AlbanianXrm_Extensions.658f55ba-c5c8-4429-8851-07b58ffddae8",
                isPreUnzipped: false,
                skipAssemblyReferences: false,
                project: project,
                packageVersions: packageVersions
            );

            var vsProject = project.Object as VSLangProj.VSProject;
            foreach (VSLangProj.Reference reference in vsProject.References)
            {
                switch (reference.Identity)
                {
                    case "Microsoft.Xrm.Sdk":
                    case "Microsoft.Crm.Sdk.Proxy":
                        reference.CopyLocal = false;
                        break;
                    default:
                        break;
                }
            }
        }

        // This method is only called for item templates,  
        // not for project templates.  
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        // This method is called after the project is created.  
        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            try
            {
                string safeprojectname;
                if (replacementsDictionary.TryGetValue("$safeprojectname$", out safeprojectname))
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // This method is only called for item templates,  
        // not for project templates.  
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
