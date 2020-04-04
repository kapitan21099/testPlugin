using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_TestPlugin.Repository
{
    class DeactivateLinkedRecords
    {
        private IOrganizationService _service;
        public DeactivateLinkedRecords(IOrganizationService service)
        {
            _service = service;        }

        public EntityCollection GetFilteredRecords(string lookUpIdOnAccaunt, string idN_NRecord)
        {
            var lookUpId = lookUpIdOnAccaunt;
            var dayCount = 2;
            var idN_N_Record = idN_NRecord;
            var qe = new QueryExpression("new_l_ki");// Instantiate QueryExpression QEnew_l_ki
            qe.TopCount = 50;
            qe.ColumnSet.AddColumns("new_name", "new_look_up_on_account_l_ki", "createdon", "new_l_kiid");// Add columns to QEnew_l_ki.ColumnSet
            qe.Criteria.AddCondition("new_look_up_on_account_l_ki", ConditionOperator.Equal, lookUpId);// Define filter QEnew_l_ki.Criteria
            qe.Criteria.AddCondition("createdon", ConditionOperator.OlderThanXDays, dayCount);// Define filter QEnew_l_ki.Criteria
            // Add link-entity QEnew_l_ki_new_new_l_ki_new_ki_n_n
            var qeEntityLinqBetween = qe.AddLink("new_new_l_ki_new_ki_n_n", "new_l_kiid", "new_l_kiid");
            // Add link-entity QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n
            var entityLink = qeEntityLinqBetween.AddLink("new_ki_n_n", "new_ki_n_nid", "new_ki_n_nid");
            // Define filter QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n.LinkCriteria
            entityLink.LinkCriteria.AddCondition("new_ki_n_nid", ConditionOperator.Equal, idN_N_Record);
            EntityCollection contacts = _service.RetrieveMultiple(qe);
            if (contacts.Entities.Count > 0)
            {
                return contacts;
            }
            return null;
        }
        public DataCollection<Entity> CountOfRecords()
        {
        var query = new QueryExpression("new_ki_n_n")
        {
            ColumnSet = new ColumnSet(true),
            Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("new_look_up_on_account_kl_n_n", ConditionOperator.NotNull)
                    }
                }
        };
        var res = _service.RetrieveMultiple(query).Entities;
            return res;
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
