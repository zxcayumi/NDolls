using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Core.Attribute;
using NDolls.Core;

namespace Test
{
    [EntityAttribute("Res_Person","PID")]
    public class PersonModel
    {
        [DataFieldAttribute("Age", "int")]
        public int Age { get; set; }

        [DataFieldAttribute("Name","nvarchar")]
        public string Name { get; set; }
    }
}
