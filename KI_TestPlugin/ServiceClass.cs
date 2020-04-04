using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI_TestPlugin.Repository;
using Microsoft.Xrm.Sdk;
using UDS.TestEduPlugin.Repository;

namespace UDS.TestEduPlugin
{
    public class ServiceClass
    {
        private IOrganizationService _service;
        public ServiceClass(IOrganizationService service)
        {
            _service = service;
        }
        public void MainMethod()
        {
            string path = @"C:\Users\Kaptan\Desktop\UDS\MyTest.txt";
            StringBuilder resLog = new StringBuilder();
            string luOn_KI_n_n = "4e4fffc8-5257-ea11-8125-00155d06f203";
            DeactivateLinkedRecords param = new DeactivateLinkedRecords(_service);
            DataCollection<Entity> countOfEntity = param.CountOfRecords();
            DeactivateLinkedRecords contactRepository = new DeactivateLinkedRecords(_service);
            List<string> listWithId = new List<string>();
            for (int i = 0; i < countOfEntity.Count; i++)
            {
                listWithId.Add(countOfEntity[i].Id.ToString());
            }
            List<Guid> listWithIdFinaly = new List<Guid>();
            foreach (var i in listWithId)
            {
                EntityCollection contacts = contactRepository.GetFilteredRecords(luOn_KI_n_n, i);
                for (int j = 0; j < contacts.Entities.Count; j++)
                {
                    if (!listWithIdFinaly.Contains(contacts.Entities[j].Id))
                    {
                        listWithIdFinaly.Add(contacts.Entities[j].Id);
                    }
                }
            }
            if (listWithIdFinaly.Count!=0)
            {
                foreach (var item in listWithIdFinaly)
                {
                    DeactivateLinkedRecords.DeactivateRecord("new_l_ki", item, _service);
                    Console.WriteLine($"Record:{item.ToString()} deactivate");
                    resLog.Append($"Record:{item.ToString()} deactivate\n");
                }
            }
            else 
            {
                Console.WriteLine("No contacts");
                resLog.Append("No contacts");
            }
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine($"\n{resLog} ");
                tw.Close();
            }
            else if (File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine($"\n{resLog} ");
                    tw.Close();
                }
            }
            Console.ReadLine();
        }
    }
}
