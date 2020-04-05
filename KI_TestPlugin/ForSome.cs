using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace UDS.TestEduPlugin
{
    public class ForSome: Plugin
    {
        public ForSome()
            : base(typeof(ForSome))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Update",
                    "new_ki_task_3",
                    ChangeLookUpFields));
        }
        private IOrganizationService _service;

        public static int StateCode { get; private set; }

        //public ForSome(IOrganizationService service)
        //{
        //    _service = service;
        //}
        public void MainMethSome()
        {
            //Guid guid = new Guid(guid);
            var QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n_new_ki_n_nid = "ad34c56c-aa6e-ea11-8125-00155d06f203";
            var qe = new QueryExpression("new_l_ki");
            qe.TopCount = 50;
            qe.ColumnSet.AddColumns("new_name", "new_l_kiid");
            var QEnew_l_ki_new_new_l_ki_new_ki_n_n = qe.AddLink("new_new_l_ki_new_ki_n_n", "new_l_kiid", "new_l_kiid");
            var QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n = QEnew_l_ki_new_new_l_ki_new_ki_n_n.AddLink("new_ki_n_n", "new_ki_n_nid", "new_ki_n_nid");
            QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n.LinkCriteria.AddCondition("new_ki_n_nid", ConditionOperator.Equal, QEnew_l_ki_new_new_l_ki_new_ki_n_n_new_ki_n_n_new_ki_n_nid);
            EntityCollection cont = _service.RetrieveMultiple(qe);
            if (cont.Entities.Count != 0)
            {
                for (int i = 0; i < cont.Entities.Count; i++)
                {
                    DeactivateRecord(cont[i].LogicalName, cont[i].Id, _service);
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
        protected void ChangeLookUpFields(LocalPluginContext localContext)
        { }
       
    }///////////////////////////
     



}

