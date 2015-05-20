using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class TestModel:MarshalByRefObject
    {
        public string Hello()
        {
            return "Hello Randy";
        }
    }

    [Serializable]
    public class SerializeModel
    {
        public int Age { get; set; }
        
        public string Name { get; set; }
    }
}
