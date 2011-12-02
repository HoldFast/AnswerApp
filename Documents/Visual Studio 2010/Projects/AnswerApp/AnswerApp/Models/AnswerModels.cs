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
    public class ViewDataUploadFilesResult
    {
        public string Name { get; set; }
        public int Length { get; set; }
    }

    public class SelectModel
    {
        [DisplayName("Textbook")]
        public string Textbook { get; set; }

        [DisplayName("MainContent_UnitsList")]
        public string MainContent_Unit { get; set; }

        [DisplayName("Unit")]
        public string Unit { get; set; }

        [DisplayName("Chapter")]
        public string Chapter { get; set; }

        [DisplayName("Section")]
        public string Section { get; set; }

        [DisplayName("Page")]
        public string Page { get; set; }

        [DisplayName("Question")]
        public string Question { get; set; }

        [DisplayName("PracticeProblemAnswer")]
        public string PracticeProblemAnswer { get; set; }

        [DisplayName("CorrectAnswer")]
        public string CorrectAnswer { get; set; }
    }

    public class UploadModel
    {
        [DisplayName("PracticeProblemAnswer")]
        public string PracticeProblemAnswer { get; set; }
    }
}
