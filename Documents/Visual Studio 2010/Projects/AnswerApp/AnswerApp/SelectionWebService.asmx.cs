//<%@ WebService Language="C#" Class="CascadingDropdown1" %>
//Web Serices
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

//Ajax Toolkit
using System.Web.Script.Services;
using AjaxControlToolkit;
//using System;
//using System.Web;
//using System.Web.Services;
using System.Web.Services.Protocols;
//using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;

//String Dictionary
using System.Collections;
using System.Collections.Specialized;

//AnswerApp
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
            //AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            //thisUser.MetaData = knownCategoryValues;
            //db.SubmitChanges();
            
            IQueryable<AnswerApp.Models.Textbook> retrieved = from theTextbooks in db.Textbooks
                                                              select theTextbooks;
            AnswerApp.Models.Textbook[] results = retrieved.ToArray<AnswerApp.Models.Textbook>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, knownCategoryValues));
            //theList.Add(new CascadingDropDownNameValue("All", "All"));
            foreach (AnswerApp.Models.Textbook theTextbook in results)
            {
                theList.Add(new CascadingDropDownNameValue(theTextbook.Title, theTextbook.Title));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetUnits(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            //AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            //thisUser.MetaData = knownCategoryValues;
            //db.SubmitChanges();
            
            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);

            IQueryable<AnswerApp.Models.Unit> retrieved = null;
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("textbook"))
                {
                    String SelectedTextbook = theEntry.Value.ToString();
                    retrieved = from theAnswers in db.Units
                                where theAnswers.Textbook_Title.Equals(SelectedTextbook)
                                select theAnswers;
                    //break;//
                }
            }
            
            AnswerApp.Models.Unit[] results = retrieved.ToArray<AnswerApp.Models.Unit>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, knownCategoryValues));
            theList.Add(new CascadingDropDownNameValue("All", "All"));
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
            //AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            //thisUser.MetaData = knownCategoryValues;
            //never fails priorto this line
            //db.SubmitChanges();//*/
            //failedpriorto this line
            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);
            //failedpriorto this line
            IQueryable<AnswerApp.Models.Chapter> retrieved = null;
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("textbook"))
                {
                    String SelectedTextbook = theEntry.Value.ToString();
                    retrieved = from theAnswers in db.Chapters
                                where theAnswers.Textbook_Title.Equals(SelectedTextbook)
                                select theAnswers;
                    //break;//
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("unit"))
                {
                    String SelectedUnit = theEntry.Value.ToString();
                    if(SelectedUnit.Equals("All"))
                    {
                        retrieved = from theAnswers in db.Chapters
                                    select theAnswers;
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Chapters
                                    where theAnswers.Unit_Title.Equals(SelectedUnit)
                                    select theAnswers;
                    }
                    //break;//
                }
            }
            
            
            AnswerApp.Models.Chapter[] results = retrieved.ToArray<AnswerApp.Models.Chapter>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, knownCategoryValues));
            //theList.Add(new CascadingDropDownNameValue(category, category));
            theList.Add(new CascadingDropDownNameValue("All", "All"));
            foreach (AnswerApp.Models.Chapter theChapter in results)
            {
                theList.Add(new CascadingDropDownNameValue(theChapter.Chapter_Title, theChapter.Chapter_Title));
            }
            //if (theList.Count == 0) { return null; }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetSections(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            
            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);

            Boolean Filtered = false;
            IQueryable<AnswerApp.Models.Section> retrieved = null;
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("textbook"))
                {
                    String SelectedTextbook = theEntry.Value.ToString();
                    retrieved = from theAnswers in db.Sections
                                where theAnswers.Textbook_Title.Equals(SelectedTextbook)
                                select theAnswers;
                    //break;//
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("unit"))
                {
                    String SelectedUnit = theEntry.Value.ToString();
                    if (SelectedUnit.Equals("All"))
                    {
                        retrieved = from theAnswers in db.Sections
                                    select theAnswers;
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Sections
                                    where theAnswers.Unit_Title.Equals(SelectedUnit)
                                    select theAnswers;
                        Filtered = true;
                    }
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("chapter"))
                {
                    String SelectedChapter = theEntry.Value.ToString();
                    if (SelectedChapter.Equals("All"))
                    {
                        if (Filtered == false)
                        {
                            retrieved = from theAnswers in db.Sections
                                        select theAnswers;
                        }
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Sections
                                    where theAnswers.Chapter_Title.Equals(SelectedChapter)
                                    select theAnswers;
                    }
                }
            }
            
            AnswerApp.Models.Section[] results = retrieved.ToArray<AnswerApp.Models.Section>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, knownCategoryValues));
            theList.Add(new CascadingDropDownNameValue("All", "All"));
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
            //AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            //thisUser.MetaData = knownCategoryValues;
            //db.SubmitChanges();
            
            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);

            Boolean Filtered = false;
            IQueryable<AnswerApp.Models.Page> retrieved = null;
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("textbook"))
                {
                    String SelectedTextbook = theEntry.Value.ToString();
                    retrieved = from theAnswers in db.Pages
                                where theAnswers.Textbook_Title.Equals(SelectedTextbook)
                                select theAnswers;
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("unit"))
                {
                    String SelectedUnit = theEntry.Value.ToString();
                    if (SelectedUnit.Equals("All"))
                    {
                        retrieved = from theAnswers in db.Pages
                                    select theAnswers;
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Pages
                                    where theAnswers.Unit_Title.Equals(SelectedUnit)
                                    select theAnswers;
                        Filtered = true;
                    }
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("chapter"))
                {
                    String SelectedChapter = theEntry.Value.ToString();
                    if (SelectedChapter.Equals("All"))
                    {
                        if (Filtered == false)
                        {
                            retrieved = from theAnswers in db.Pages
                                        select theAnswers;
                        }
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Pages
                                    where theAnswers.Chapter_Title.Equals(SelectedChapter)
                                    select theAnswers;
                    }
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("section"))
                {
                    String SelectedSection = theEntry.Value.ToString();
                    if (SelectedSection.Equals("All"))
                    {
                        if (Filtered == false)
                        {
                            if (Filtered == false)
                            {
                                retrieved = from theAnswers in db.Pages
                                            select theAnswers;
                            }
                        }
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Pages
                                    where theAnswers.Section_Title.Equals(SelectedSection)
                                    select theAnswers;
                    }
                }
            }

            AnswerApp.Models.Page[] results = retrieved.ToArray<AnswerApp.Models.Page>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, knownCategoryValues));
            theList.Add(new CascadingDropDownNameValue("All", "All"));
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
            //AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            //thisUser.MetaData = knownCategoryValues;
            //db.SubmitChanges();
            
            StringDictionary knownCatagories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            DictionaryEntry[] CatagoryArray = new DictionaryEntry[knownCatagories.Count];
            knownCatagories.CopyTo(CatagoryArray, 0);

            Boolean Filtered = false;
            IQueryable<AnswerApp.Models.Question> retrieved = null;
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("textbook"))
                {
                    String SelectedTextbook = theEntry.Value.ToString();
                    retrieved = from theAnswers in db.Questions
                                where theAnswers.Textbook_Title.Equals(SelectedTextbook)
                                select theAnswers;
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("unit"))
                {
                    String SelectedUnit = theEntry.Value.ToString();
                    if (SelectedUnit.Equals("All"))
                    {
                            retrieved = from theAnswers in db.Questions
                                        select theAnswers;
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Questions
                                    where theAnswers.Unit_Title.Equals(SelectedUnit)
                                    select theAnswers;
                        Filtered = true;
                    }
                }
            } 
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("chapter"))
                {
                    String SelectedChapter = theEntry.Value.ToString();
                    if (SelectedChapter.Equals("All"))
                    {
                        if (Filtered == false)
                        {
                            retrieved = from theAnswers in db.Questions
                                        select theAnswers;
                        }
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Questions
                                    where theAnswers.Chapter_Title.Equals(SelectedChapter)
                                    select theAnswers;
                        Filtered = true;
                    }
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("section"))
                {
                    String SelectedSection = theEntry.Value.ToString();
                    if (SelectedSection.Equals("All"))
                    {
                        if (Filtered == false)
                        {
                            retrieved = from theAnswers in db.Questions
                                        select theAnswers;
                        }
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Questions
                                    where theAnswers.Section_Title.Equals(SelectedSection)
                                    select theAnswers;
                        Filtered = true;
                    }
                }
            }
            foreach (DictionaryEntry theEntry in CatagoryArray)
            {
                if (theEntry.Key.ToString().Equals("page"))
                {
                    String SelectedPage = theEntry.Value.ToString();
                    if (SelectedPage.Equals("All"))
                    {
                        if (Filtered == false)
                        {
                            retrieved = from theAnswers in db.Questions
                                        select theAnswers;
                        }
                    }
                    else
                    {
                        retrieved = from theAnswers in db.Questions
                                    where theAnswers.Page_Number.Equals(SelectedPage)
                                    select theAnswers;
                        Filtered = true;
                    }
                }
            }
            
            AnswerApp.Models.Question[] results = retrieved.ToArray<AnswerApp.Models.Question>();
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            //theList.Add(new CascadingDropDownNameValue(knownCategoryValues, knownCategoryValues));
            theList.Add(new CascadingDropDownNameValue("All", "All"));
            foreach (AnswerApp.Models.Question theQuestion in results)
            {
                theList.Add(new CascadingDropDownNameValue(theQuestion.Question_Number, theQuestion.Question_Number));
            }
            return theList.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] SetQuestions(string knownCategoryValues, string category)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            thisUser.MetaData = knownCategoryValues;
            db.SubmitChanges();
            
            List<CascadingDropDownNameValue> theList = new List<CascadingDropDownNameValue>();
            
            theList.Add(new CascadingDropDownNameValue(thisUser.UserName, thisUser.UserName));
            return theList.ToArray();
        }
    }
}

//End