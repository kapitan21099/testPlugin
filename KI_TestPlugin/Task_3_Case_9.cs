using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.TestEduPlugin
{
	public class Task_3_Case_9 : Plugin
	{
		public Task_3_Case_9() : base(typeof(Task_3_Case_9))
		{
			base.RegisteredEvents
				.Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Update",
					"new_ki_task_3",
					ChangeLookUpFields));			
		}

		protected void ChangeLookUpFields(LocalPluginContext localContext)
		{
			if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
			{
				throw new InvalidPluginExecutionException($"{nameof(localContext)} doesn`t contain 'Target'");
			}
			var target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];			
			var contactId = target.GetAttributeValue<EntityReference>("new_look_up_on_contact");
			var accauntId = target.GetAttributeValue<EntityReference>("new_look_up_onaccaunt");			
			if (target.Contains("new_look_up_onaccaunt"))
			{
				if (accauntId is null)
					target["new_look_up_on_contact"] = null;
				else
				{
					var contactOfTargetClient = localContext.OrganizationService.Retrieve(accauntId.LogicalName, accauntId.Id, new ColumnSet("primarycontactid"));
					target["new_look_up_on_contact"] = contactOfTargetClient.GetAttributeValue<EntityReference>("new_look_up_onaccaunt");
				}
			}
			else if (target.Contains("new_look_up_on_contact"))
			{
				if (contactId is null)
					target["new_look_up_onaccaunt"] = null;
				else
				{
					var clientOfTargetContact = localContext.OrganizationService.Retrieve(contactId.LogicalName, contactId.Id, new ColumnSet("parentcustomerid"));
					target["new_look_up_onaccaunt"] = clientOfTargetContact.GetAttributeValue<EntityReference>("parentcustomerid");
				}
			}
		}
	}
}