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

using System.Collections;
using System.Collections.Specialized;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnswerApp.Models;
using System.IO;
using System.Net.Mime;



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
        public CascadingDropDownNameValue[] GetTextbooks(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            IQueryable<AnswerApp.Models.Textbook> retrieved = from theTextbooks in db.Textbooks
                                                          //where theTextbooks.Title.Equals("Mathematics 10")//Textbook_Title)
                                                          select theTextbooks;
            AnswerApp.Models.Textbook[] results = retrieved.ToArray<AnswerApp.Models.Textbook>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Textbook theTextbook in results)
            {
                theList.Add(new CascadingDropDownNameValue(theTextbook.Title, theTextbook.Title));
            }
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, "1"));
            //theList.Add(new CascadingDropDownNameValue(category, "2"));
            return theList.ToArray();
        }
        [WebMethod]
        public CascadingDropDownNameValue[] GetUnits(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            String SelectedTextbook = "Error 1";
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("textbook"))
                {
                    SelectedTextbook = theEntry.Value.ToString();
                }
            }
            
            IQueryable<AnswerApp.Models.Unit> retrieved = from theUnits in db.Units
                                                          where theUnits.Textbook_Title.Equals(SelectedTextbook)
                                                          select theUnits;
            AnswerApp.Models.Unit[] results = retrieved.ToArray<AnswerApp.Models.Unit>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Unit theUnit in results)
            {
                theList.Add(new CascadingDropDownNameValue(theUnit.Unit_Title, theUnit.Unit_Title));
            }
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, "1"));
            //theList.Add(new CascadingDropDownNameValue(category, "2"));
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetChapters(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            String SelectedUnit = "Error 1";
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("unit"))
                {
                    SelectedUnit = theEntry.Value.ToString();
                }
            }
            
            IQueryable<AnswerApp.Models.Chapter> retrieved = from theChapters in db.Chapters
                                                             where theChapters.Unit_Title.Equals(SelectedUnit)//Textbook_Title)
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

            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            String SelectedChapter = "Error 1";
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("chapter"))
                {
                    SelectedChapter = theEntry.Value.ToString();
                }
            }

            IQueryable<AnswerApp.Models.Section> retrieved = from theSections in db.Sections
                                                              where theSections.Chapter_Title.Equals(SelectedChapter)//Textbook_Title)
                                                              select theSections;
            AnswerApp.Models.Section[] results = retrieved.ToArray<AnswerApp.Models.Section>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Section theSection in results)
            {
                theList.Add(new CascadingDropDownNameValue(theSection.Section_Title, theSection.Section_Title));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetPages(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            String SelectedSection = "Error 1";
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("section"))
                {
                    SelectedSection = theEntry.Value.ToString();
                }
            }
            
            IQueryable<AnswerApp.Models.Page> retrieved = from theAnswers in db.Pages
                                                              where theAnswers.Section_Title.Equals(SelectedSection)//Textbook_Title)
                                                              select theAnswers;
            AnswerApp.Models.Page[] results = retrieved.ToArray<AnswerApp.Models.Page>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Page thePage in results)
            {
                theList.Add(new CascadingDropDownNameValue(thePage.Page_Number, thePage.Page_Number));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetQuestions(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            String SelectedPage = "Error 1";
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("page"))
                {
                    SelectedPage = theEntry.Value.ToString();
                }
            }
            
            IQueryable<AnswerApp.Models.Question> retrieved = from theAnswers in db.Questions
                                                              where theAnswers.Page_Number.Equals(SelectedPage)//Textbook_Title)
                                                              select theAnswers;
            AnswerApp.Models.Question[] results = retrieved.ToArray<AnswerApp.Models.Question>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            foreach (AnswerApp.Models.Question theQuestion in results)
            {
                theList.Add(new CascadingDropDownNameValue(theQuestion.Question_Number, theQuestion.Question_Number));
            }
            //return RedirectToAction("Answers", "ViewAnswer");
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] SetQuestions(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            String SelectedQuestion = "Error: No Question Selected";
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("question"))
                {
                    SelectedQuestion = theEntry.Value.ToString();
                }
            }

            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            thisUser.MetaData = knownCategoryValues;
            db.SubmitChanges();
            
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            
            theList.Add(new CascadingDropDownNameValue(thisUser.UserName, thisUser.UserName));
            //return RedirectToAction("Answers", "ViewAnswer");
            return theList.ToArray();
        }
    }
}
