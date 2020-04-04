using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.TestEduPlugin
{
    public class ForSome : Plugin
    {
        public ForSome()
            : base(typeof(ForSome))
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

            }///////////////////////////
            IOrganizationService service = localContext.OrganizationService;
             // Define Condition Values
            var lookUpId = "4e4fffc8-5257-ea11-8125-00155d06f203";
            var dayCount = 2;
            var idN_N_Record = "2cbcc367-ae6e-ea11-8125-00155d06f203";

            // Instantiate QueryExpression QEnew_l_ki
            var qe = new QueryExpression("new_l_ki");
            qe.TopCount = 50;
            // Add columns to QEnew_l_ki.ColumnSet
            qe.ColumnSet.AddColumns("new_name", "new_look_up_on_account_l_ki", "createdon", "new_l_kiid");
            // Define filter QEnew_l_ki.Criteria
            qe.Criteria.AddCondition("new_look_up_on_account_l_ki", ConditionOperator.Equal, lookUpId);
            qe.Criteria.AddCondition("createdon", ConditionOperator.OlderThanXDays, dayCount);
            // Add link-entity QEnew_l_ki_new_new_l_ki_new_ki_n_n
            var QEnew_l_ki_new_new_l_ki_new_ki_n_n = qe.AddLink("new_new_l_ki_new_ki_n_n", "new_l_kiid", "new_l_kiid");
            // Add link-entity QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n
            var QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n = QEnew_l_ki_new_new_l_ki_new_ki_n_n.AddLink("new_ki_n_n", "new_ki_n_nid", "new_ki_n_nid");

            // Define filter QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n.LinkCriteria
            QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n.LinkCriteria.AddCondition("new_ki_n_nid", ConditionOperator.Equal, idN_N_Record);
            EntityCollection contacts = service.RetrieveMultiple(qe);
            //////////////////////////
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            Entity parentCustonerId, primaryContactId;
            EntityReference lookupOnContact, lookupOnAccaunt;
            if (target.Contains("new_look_up_on_contact"))
                lookupOnContact = target.GetAttributeValue<EntityReference>("new_look_up_on_contact");
            else
                lookupOnContact = null;
            if (target.Contains("new_look_up_onaccaunt"))
                lookupOnAccaunt = target.GetAttributeValue<EntityReference>("new_look_up_onaccaunt");
            else
                lookupOnAccaunt = null;
            if (lookupOnContact is null)
            {
                parentCustonerId = service.Retrieve(lookupOnContact.LogicalName, lookupOnContact.Id, new ColumnSet("parentcustomerid1"));
                if (parentCustonerId is null)
                    target["new_look_up_onaccaunt"] = null;
                else
                    target["new_look_up_onaccaunt"] = parentCustonerId.GetAttributeValue<EntityReference>("parentcustomerid1");
            }
            else if (lookupOnAccaunt is null)
            {
                primaryContactId = service.Retrieve(lookupOnAccaunt.LogicalName, lookupOnAccaunt.Id, new ColumnSet("primarycontactid"));
                if (primaryContactId is null)
                    target["new_look_up_on_contact"] = null;
                else
                    target["new_look_up_on_contact"] = primaryContactId.GetAttributeValue<EntityReference>("primaryContactId");
            }            
        }
    }
}
