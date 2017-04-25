using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeIt.Models
{
    //Used as a model for the All View 
    public class AllCodesModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string CodeTitle { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}