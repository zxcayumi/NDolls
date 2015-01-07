using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using NDolls.Data.Attribute;
using NDolls.Data.Entity;
namespace Test.Model
{
    //Test_Table
    [Entity("Test_Table", "key1,key2")]
    public class Test_Table : EntityBase
    {
        /// <summary>
        /// id
        /// </summary>		
        [DataField("id", "int", true)]
        public int id { get; set; }

        /// <summary>
        /// key1
        /// </summary>		
        [DataField("key1", "nvarchar")]
        [Validate("")]
        public string key1 { get; set; }

        /// <summary>
        /// key2
        /// </summary>		
        [DataField("key2", "nvarchar")]
        [Validate("")]

        public string key2 { get; set; }

        /// <summary>
        /// memo
        /// </summary>		
        [DataField("memo", "nvarchar")]
        public string memo { get; set; }

    }
}