using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    public class SourceModelOne
    {
        public string SomeInitialProperty
        {
            get
            {
                return "I was there at the beginning.";
            }
        }

        [DisplayName("AddedProp_Decorated")]
        public int AddedProp
        {
            get;
            set;
        }

        [DisplayName("OtherAddedProp_Decorated")]
        public string OtherAddedProp
        {
            get;
            set;
        }
    }
}