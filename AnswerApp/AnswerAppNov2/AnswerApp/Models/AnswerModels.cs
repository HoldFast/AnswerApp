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
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Date of Birth (\"yyyy-mm-dd\")")]
        public string DateOfBirth { get; set; }

        [DisplayName("Health Care Number")]
        public string HealthCareNumber { get; set; }

        [DisplayName("Textbook")]
        public string Textbook { get; set; }

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

        /*[DisplayName("PracticeProblemAnswerA")]
        public string PracticeProblemAnswerA { get; set; }

        [DisplayName("PracticeProblemAnswerB")]
        public string PracticeProblemAnswerB { get; set; }

        [DisplayName("PracticeProblemAnswerC")]
        public string PracticeProblemAnswerC { get; set; }

        [DisplayName("PracticeProblemAnswerD")]
        public string PracticeProblemAnswerD { get; set; }//*/
    }

    public class UploadModel
    {
        
    }
}
