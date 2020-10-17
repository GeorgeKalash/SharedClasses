using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public class JsonProtocol
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
    }
}
