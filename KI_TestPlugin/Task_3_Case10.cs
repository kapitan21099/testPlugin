using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;

namespace KI_TestPlugin
{

    public class Task_3_Case10 : CodeActivity
    {
        #region Workflow parameters
        [ReferenceTarget("contact")]
        [RequiredArgument]
        [Input("InputContact")]
        public InArgument<EntityReference> ContactParam { get; set; }
        /*
            [RequiredArgument]
            [Input("IntToAdd")]
            public InArgument<int> IntParam { get; set; }
        */
        [RequiredArgument]
        [Output("OuterEmaill")]
        public OutArgument<string> EmailParam { get; set; }

        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {
            string email;
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var contactId = ContactParam.Get<EntityReference>(executionContext);
            var updateEntity = new Entity(context.PrimaryEntityName, context.PrimaryEntityId);
            if (contactId is null)
                EmailParam.Set(executionContext, "Contact don't have email");
            var contactEntity = service.Retrieve(contactId.LogicalName, contactId.Id, new ColumnSet("emailaddress1"));
            if (contactEntity.Contains("emailaddress1"))
            {
                email = contactEntity.GetAttributeValue<string>("emailaddress1");
            }
            else
            {
                email = "Contact don't have email";
                EmailParam.Set(executionContext, email); //throw new ArgumentNullException("Result of retrive is null!!");
            }
            if (email is null)
                EmailParam.Set(executionContext, "Contact don't have email");
            else
                EmailParam.Set(executionContext, email);
            service.Update(updateEntity);
        }

    }
}

