using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.TestEduPlugin
{
    public class Task_3_Case8_2_Tracing : Plugin
    {
        public Task_3_Case8_2_Tracing()
            : base(typeof(Task_3_Case8_2_Tracing))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Create",
                    "new_ki_task_3",
                    GenerateValue));
        }

        protected void GenerateValue(LocalPluginContext localContext)
        {
            localContext.Trace("start to tracing");
            try
            {
                if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
                {
                    return;
                }
                localContext.Trace($"Tracing -input parameters count {localContext.PluginExecutionContext.InputParameters.Count.ToString()}");
                Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
                localContext.Trace($"Tracing - Entity Attribute Count:{target.Attributes.Count.ToString()}");
                foreach (var atr in target.Attributes)
                {
                    localContext.Trace($"Tracing key {target.Attributes.Keys} - value{target.Attributes.Values}");
                }
                localContext.Trace("End Tracing");
                localContext.OrganizationService.Update(target);
            }
            catch (Exception ex)
            {
                localContext.Trace(ex.ToString());
                throw;
            }
                     

        }
    }
}
