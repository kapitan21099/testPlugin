using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.TestEduPlugin
{
    public class Task_3_Case7_1 : Plugin
    {
        public Task_3_Case7_1()
            : base(typeof(Task_3_Case7_1))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Create",
                    "new_ki_task_3",
                    GenerateValue));
        }

        protected void GenerateValue(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            localContext.Trace("");          
            Random rnd = new Random();
            target["new_generic_text_field"] = rnd.Next(1, 10000).ToString();
            localContext.OrganizationService.Update(target);         
           
        }
    }
}
