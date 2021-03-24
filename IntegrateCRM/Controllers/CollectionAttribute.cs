using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrateCRM.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionAttribute : Attribute
    {
        public CollectionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
