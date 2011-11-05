using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using System.ComponentModel;
using System.Linq;
using System.Web;
using AnswerApp.Models;

namespace AnswerApp.Models
{
    /*public class Textbook
    {
        public String TextbookTitle;

        public String TextbookPercentage;
    }//*/

    public class HomeModel// : AnswerApp.Models.SelectModel
    {
        public bool PurchaseMoreSolutions { get; set; }

        public bool PurchaseForNewBook { get; set; }

        //public List<Textbook> UserTextbooks { get; set; }
    }
    
    public class PayModel
    {
        public string FileName { get; set; }
    }
}
