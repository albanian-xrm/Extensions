using Microsoft.Xrm.Sdk;
using System;

namespace AlbanianXrm
{
    public partial class PluginBase
    {
        protected interface IContext
        {
            ITracingService TracingService { get; }
            IPluginExecutionContext PluginExecutionContext { get; }
            IOrganizationService GetOrganizationService();
            IOrganizationService GetOrganizationService(Guid systemuserid);
            IOrganizationService GetOrganizationServiceSystem();
        }
    }
}
