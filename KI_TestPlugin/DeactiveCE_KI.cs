using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.TestEduPlugin
{
	public class DeactiveCE_KI : Plugin
	{
		public DeactiveCE_KI() : base(typeof(DeactiveCE_KI))
		{
			base.RegisteredEvents
				.Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "new_DeactivChaild_KI",
                    "new_ki_n_n",
					ChangeLookDeactiveChildEntity));
		}

		protected void ChangeLookDeactiveChildEntity(LocalPluginContext localContext)
		{
			IOrganizationService service = localContext.OrganizationService;
			if (!localContext.PluginExecutionContext.InputParameters.Contains("deactiv_chaild_KI"))
			{
				throw new InvalidPluginExecutionException($"{nameof(localContext)} doesn`t contain 'deactiv_chaild_KI'");
			}
			var entCollChaild = (EntityCollection)localContext.PluginExecutionContext.InputParameters["deactiv_chaild_KI"];
            if (entCollChaild.Entities.Count != 0)
            {
                for (int i = 0; i < entCollChaild.Entities.Count; i++)
                {
                    DeactivateRecord(entCollChaild[i].LogicalName, entCollChaild[i].Id, service);
                }


            }
        }
        public static void DeactivateRecord(string entityName, Guid recordId, IOrganizationService organizationService)
        {
            var cols = new ColumnSet(new[] { "statecode", "statuscode" });
            //Check if it is Active or not
            var entity = organizationService.Retrieve(entityName, recordId, cols);
            if (entity != null && entity.GetAttributeValue<OptionSetValue>("statecode").Value == 0)
            {
                //StateCode = 1 and StatusCode = 2 for deactivating Account or Contact
                SetStateRequest setStateRequest = new SetStateRequest()
                {
                    EntityMoniker = new EntityReference
                    {
                        Id = recordId,
                        LogicalName = entityName,
                    },
                    State = new OptionSetValue(1),
                    Status = new OptionSetValue(2)
                };
                organizationService.Execute(setStateRequest);
            }
        }        
    }
	}
