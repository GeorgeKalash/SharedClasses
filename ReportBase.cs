using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace SharedClasses
{
    public class ReportBase
    {
        public class QryStructure<T>
        {
            public int count;
            public List<T> list;
        }
        public class GetStructure<T>
        {
            public T record;
            public int statusId;
            public string message;
        }

        public string data;
        public SharedClasses.Language languageId;
        public Dictionary<string, string> labels;
        private string rootAPI;
        private int? accountId;

        public string companyLogoUrl
        {
            get
            {
                return accountId == null ? null : string.Format("https://s3-eu-west-1.amazonaws.com/{0}/20203/{1}/1-0.png", rootAPI, accountId);
            }
            set
            {

            }
        }

        public ReportBase()
        {
        }

        public void setReportData(string _jsonBody)
        {
            data = _jsonBody;
        }
        public void setAccount(int _accountId)
        {
            accountId = _accountId;
        }
        public void setLanguage(string fileName, SharedClasses.Language _languageId)
        {
            languageId = _languageId;
            loadDict(fileName);
        }

        public ReportBase(string _rootAPI) : this()
        {
            rootAPI = _rootAPI;
        }

        public ReportBase(string _rootAPI, int _accountId) : this(_rootAPI)
        {
            setAccount(_accountId);
        }

        private void loadDict(string fileName)
        {
            string path = ConfigurationManager.AppSettings["reports-labels-folder"] + fileName + ".xml";
            labels = SharedClasses.XMLTools.loadDict(path, "L"+((short)languageId).ToString()); 
        }
    }
}
