using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;

namespace AlbanianXrm
{
    public partial class WorkflowBase
    {
        protected interface IContext
        {
            ITracingService TracingService { get; }
            IWorkflowContext WorkflowContext { get; }
            IArguments Arguments { get; }
            IOrganizationService GetOrganizationService();
            IOrganizationService GetOrganizationService(Guid systemuserid);
            IOrganizationService GetOrganizationServiceSystem();
        }
    }
}
