using System.Collections.Generic;

namespace SharedClasses
{
    public class JsonProtocol
    {
        public class StatusCheck
        {
            public int statusId;
        }
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
        public class ErrorStructure
        {
            public int statusId;
            public string error;
            public int logId;
        }
    }       
}
