using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.TestEduPlugin
{
    public class Task_3_Case7_2 : Plugin
    {
        public Task_3_Case7_2()
            : base(typeof(Task_3_Case7_2))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Update",
                    "new_ki_task_3",
                    IsContainEmailInLookUp));
        }

        protected void IsContainEmailInLookUp(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            localContext.Trace("");
            IOrganizationService service = localContext.OrganizationService;
            EntityReference lookupRef = target.GetAttributeValue<EntityReference>("new_look_up_onaccaunt");
            string noException = "The client meets the requirements";
            string haveException = "Client don't have email";
            var email = service.Retrieve(lookupRef.LogicalName, lookupRef.Id, new ColumnSet(true));
            if (email.Contains("emailaddress1"))
            {                
                target["new_field_for_exeption"] = noException; 
            }
            else
            {
                target["new_field_for_exeption"] = haveException;
            }            
        }
    }
}
