//<%@ WebService Language="C#" Class="CascadingDropdown1" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Script.Services;
using AjaxControlToolkit;
//using System;
//using System.Web;
//using System.Web.Services;
using System.Web.Services.Protocols;
//using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;


namespace AnswerApp
{
    /// <summary>
    /// Summary description for SelectionWebService
    /// </summary>
    [WebService(Namespace = "http://solvation.ca/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class SelectionWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public CascadingDropDownNameValue[] GetUnits(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            IQueryable<AnswerApp.Models.Unit> retrieved = from theUnits in db.Units
                                                              where theUnits.Textbook_Title.Equals("Mathematics 10")//Textbook_Title)
                                                              select theUnits;
            AnswerApp.Models.Unit[] results = retrieved.ToArray<AnswerApp.Models.Unit>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Unit theUnit in results)
            {
                theList.Add(new CascadingDropDownNameValue(theUnit.Unit_Title, theUnit.Unit_Title));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetChapters(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            IQueryable<AnswerApp.Models.Chapter> retrieved = from theChapters in db.Chapters
                                                             where theChapters.Textbook_Title.Equals("Mathematics 10")//Textbook_Title)
                                                             select theChapters;
            AnswerApp.Models.Chapter[] results = retrieved.ToArray<AnswerApp.Models.Chapter>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Chapter theChapter in results)
            {
                theList.Add(new CascadingDropDownNameValue(theChapter.Chapter_Title, theChapter.Chapter_Title));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetSections(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            IQueryable<AnswerApp.Models.Question> retrieved = from theAnswers in db.Questions
                                                              where theAnswers.Textbook_Title.Equals("Mathematics 10")//Textbook_Title)
                                                              select theAnswers;
            AnswerApp.Models.Question[] results = retrieved.ToArray<AnswerApp.Models.Question>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Question theQuestion in results)
            {
                theList.Add(new CascadingDropDownNameValue(theQuestion.Question_Number, theQuestion.Question_Number));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetPages(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            IQueryable<AnswerApp.Models.Question> retrieved = from theAnswers in db.Questions
                                                              where theAnswers.Textbook_Title.Equals("Mathematics 10")//Textbook_Title)
                                                              select theAnswers;
            AnswerApp.Models.Question[] results = retrieved.ToArray<AnswerApp.Models.Question>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Question theQuestion in results)
            {
                theList.Add(new CascadingDropDownNameValue(theQuestion.Question_Number, theQuestion.Question_Number));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetQuestions(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            IQueryable<AnswerApp.Models.Question> retrieved = from theAnswers in db.Questions
                                                              where theAnswers.Textbook_Title.Equals("Mathematics 10")//Textbook_Title)
                                                              select theAnswers;
            AnswerApp.Models.Question[] results = retrieved.ToArray<AnswerApp.Models.Question>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Question theQuestion in results)
            {
                theList.Add(new CascadingDropDownNameValue(theQuestion.Question_Number, theQuestion.Question_Number));
            }
            return theList.ToArray();
        }
    }
}
