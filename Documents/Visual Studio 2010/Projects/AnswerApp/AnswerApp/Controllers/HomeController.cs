using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using AnswerApp.Models;


namespace AnswerApp.Controllers
{
    public class HomeController : Controller
    {

        // data GET service
        public JsonResult getUser(int id)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            User user = db.Users.Single(u => u.Unique_Id == 4);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(HomeModel model)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Message = "The Answer App";
                //String Username = User.Identity.Name;
                ViewBag.Username = User.Identity.Name;//Username;
                
                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));

                if (thisUser == null) { return RedirectToAction("LogOn", "Account"); }//This should never happen

                AnswerApp.Models.SelectModel fakeModel = new AnswerApp.Models.SelectModel();
                fakeModel.Textbook = "All";
                fakeModel.Unit = "All";
                fakeModel.Chapter = "All";
                fakeModel.Section = "All";
                fakeModel.Page = "All";
                fakeModel.Question = "All";
                ViewData["SelectionList"] = GenerateSelectionList(fakeModel);
                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account"); //return View();
            }
        }

        [HttpPost]
        public ActionResult Index(HomeModel model, string returnUrl)
        {
            //Extract the known catagory values from the user's meta data
            AnswerApp.Models.AnswerAppDataContext db = new AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            String knownCategoryValues = thisUser.MetaData;
            
            //Convert to proper format
            knownCategoryValues = knownCategoryValues.Replace("Textbook:", "");
            knownCategoryValues = knownCategoryValues.Replace("Unit:", "");
            knownCategoryValues = knownCategoryValues.Replace("Chapter:", "");
            knownCategoryValues = knownCategoryValues.Replace("Section:", "");
            knownCategoryValues = knownCategoryValues.Replace("Page:", "");
            knownCategoryValues = knownCategoryValues.Replace("Question:", "");
            
            //Disect the file name for it's file properties
            String[] properties = knownCategoryValues.Split(new char[1] { ';' });
            String Textbook_Title = null;
            String Unit_Title = null;
            String Chapter_Title = null;
            String Section_Title = null;
            String Page_Number = null;
            String Question_Number = null;
            if (properties.Length > 0) { Textbook_Title = properties[0]; }
            if (properties.Length > 1) { Unit_Title = properties[1]; }
            if (properties.Length > 2) { Chapter_Title = properties[2]; }
            if (properties.Length > 3) { Section_Title = properties[3]; }
            if (properties.Length > 4) { Page_Number = properties[4]; }
            if (properties.Length > 5) { Question_Number = properties[5]; }//.Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name

            //Populate parent groupings based on childgroupings
            if (!Question_Number.Equals("All") && Page_Number.Equals("All"))
            {
                Page_Number = db.Questions.Single<Question>(q => q.Question_Number.Equals(Question_Number)).Page_Number;
            }
            if (!Page_Number.Equals("All") && Section_Title.Equals("All"))
            {
                Section_Title = db.Pages.Single<Page>(p => p.Page_Number.Equals(Page_Number)).Section_Title;
            }
            if (!Section_Title.Equals("All") && Chapter_Title.Equals("All"))
            {
                Chapter_Title = db.Sections.Single<Section>(s => s.Section_Title.Equals(Section_Title)).Chapter_Title;
            }
            if (!Chapter_Title.Equals("All") && Unit_Title.Equals("All"))
            {
                Unit_Title = db.Chapters.Single<Chapter>(c => c.Chapter_Title.Equals(Chapter_Title)).Unit_Title;
            }

            AnswerApp.Models.SelectModel theSelectModel = new SelectModel();
            theSelectModel.Textbook = Textbook_Title;
            theSelectModel.Unit = Unit_Title;
            theSelectModel.Chapter = Chapter_Title;
            theSelectModel.Section = Section_Title;
            theSelectModel.Page = Page_Number;
            theSelectModel.Question = Question_Number;

            return RedirectToAction("ViewAnswer/" + User.Identity.Name, "Answers", theSelectModel);
        }

        public ActionResult ResourceUnavailable(HomeModel model)
        {
            return View();
        }

        public String GenerateSelectionList(SelectModel model)
        {
            String SelectionList = "";

            AnswerApp.Models.SelectModel newModel = new AnswerApp.Models.SelectModel();
            newModel.Textbook = model.Textbook;
            newModel.Unit = model.Unit;
            newModel.Chapter = model.Chapter;
            newModel.Section = model.Section;
            newModel.Page = model.Page;
            newModel.Question = model.Question;
            AnswerApp.Controllers.AnswersController thisAnswerController = new AnswerApp.Controllers.AnswersController();
                

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single<User>(u => u.UserName.Equals(User.Identity.Name));
            String[] ThisUsersAnswers = thisUser.Answers.Split(new char[1] { ';' });
            if (model.Textbook.Equals("All"))//All Textbooks have been specified
            {
                IQueryable<AnswerApp.Models.Textbook> retrieved = from theAnswers in db.Textbooks
                                                                  select theAnswers;
                AnswerApp.Models.Textbook[] results = retrieved.ToArray<AnswerApp.Models.Textbook>();
                foreach (Textbook theTextbook in results)
                {
                    model.Textbook = theTextbook.Title;
                    if (UserHasAccess(User.Identity.Name, model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf"))
                    {
                        SelectionList += "<a href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theTextbook.Title + "</a><br />" + GenerateSelectionList(model);
                    }
                    else
                    {
                        foreach (String thisAnswer in ThisUsersAnswers)
                        {
                            String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                            if (!(theseProperties.Length < 2))
                            {
                                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                                thisModel.Textbook = theseProperties[0];
                                thisModel.Unit = theseProperties[1];
                                thisModel.Chapter = theseProperties[2];
                                thisModel.Section = theseProperties[3];
                                thisModel.Page = theseProperties[4];
                                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                                if (thisModel.Textbook.Equals(model.Textbook)) { SelectionList += "<a style=\"color: #FF0000\" href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theTextbook.Title + "</a><br />" + GenerateSelectionList(model); break; }
                            }
                            else { SelectionList += GenerateSelectionList(model); }
                        }
                    }
                    model.Textbook = "All";
                }
            }
            if (model.Unit.Equals("All"))//All units have been specified
            {
                IQueryable<AnswerApp.Models.Unit> retrieved = from theAnswers in db.Units
                                                              where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                              select theAnswers;
                AnswerApp.Models.Unit[] results = retrieved.ToArray<AnswerApp.Models.Unit>();
                foreach (Unit theUnit in results)
                {
                    model.Unit = theUnit.Unit_Title;
                    if (UserHasAccess(User.Identity.Name, model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf"))
                    {
                        SelectionList += "&nbsp;<a href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theUnit.Unit_Title + "</a><br />" + GenerateSelectionList(model);
                    }
                    else
                    {
                        foreach (String thisAnswer in ThisUsersAnswers)
                        {
                            String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                            if (!(theseProperties.Length < 2))
                            {
                                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                                thisModel.Textbook = theseProperties[0];
                                thisModel.Unit = theseProperties[1];
                                thisModel.Chapter = theseProperties[2];
                                thisModel.Section = theseProperties[3];
                                thisModel.Page = theseProperties[4];
                                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                                if (thisModel.Unit.Equals(model.Unit)) { SelectionList += "&nbsp;<a style=\"color: #FF0000\" href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theUnit.Unit_Title + "</a><br />" + GenerateSelectionList(model); break; }
                            }
                            else { SelectionList += GenerateSelectionList(model); }
                        }
                    }
                    model.Unit = "All";
                }
            }
            else if (model.Chapter.Equals("All"))//Only one unit has been specified
            {
                IQueryable<AnswerApp.Models.Chapter> retrieved = from theAnswers in db.Chapters
                                                                 where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                                 && theAnswers.Unit_Title.Equals(model.Unit)
                                                                 select theAnswers;
                AnswerApp.Models.Chapter[] results = retrieved.ToArray<AnswerApp.Models.Chapter>();
                foreach (Chapter theChapter in results)
                {
                    model.Chapter = theChapter.Chapter_Title;
                    if (UserHasAccess(User.Identity.Name, model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf"))
                    {
                        SelectionList += "&nbsp;&nbsp;<a href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theChapter.Chapter_Title + "</a><br />" + GenerateSelectionList(model);
                    }
                    else
                    {
                        foreach (String thisAnswer in ThisUsersAnswers)
                        {
                            String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                            if (!(theseProperties.Length < 2))
                            {
                                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                                thisModel.Textbook = theseProperties[0];
                                thisModel.Unit = theseProperties[1];
                                thisModel.Chapter = theseProperties[2];
                                thisModel.Section = theseProperties[3];
                                thisModel.Page = theseProperties[4];
                                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                                if (thisModel.Chapter.Equals(model.Chapter)) { SelectionList += "&nbsp;&nbsp;<a style=\"color: #FF0000\" href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theChapter.Chapter_Title + "</a><br />" + GenerateSelectionList(model); break; }
                            }
                            else { SelectionList += GenerateSelectionList(model); }
                        }
                    }
                    model.Chapter = "All";
                }
            }
            else if (model.Section.Equals("All"))//Only one unit has been specified
            {
                IQueryable<AnswerApp.Models.Section> retrieved = from theAnswers in db.Sections
                                                                 where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                                 && theAnswers.Unit_Title.Equals(model.Unit)
                                                                 && theAnswers.Chapter_Title.Equals(model.Chapter)
                                                                 select theAnswers;
                AnswerApp.Models.Section[] results = retrieved.ToArray<AnswerApp.Models.Section>();
                foreach (Section theSection in results)
                {
                    model.Section = theSection.Section_Title;
                    //newModel.Section = theSection.Section_Title
                    if (UserHasAccess(User.Identity.Name, model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf"))
                    {
                        SelectionList += "&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theSection.Section_Title + "</a><br />" + GenerateSelectionList(model);
                    }
                    else
                    {
                        foreach (String thisAnswer in ThisUsersAnswers)
                        {
                            String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                            if (!(theseProperties.Length < 2))
                            {
                                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                                thisModel.Textbook = theseProperties[0];
                                thisModel.Unit = theseProperties[1];
                                thisModel.Chapter = theseProperties[2];
                                thisModel.Section = theseProperties[3];
                                thisModel.Page = theseProperties[4];
                                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                                if (thisModel.Section.Equals(model.Section)) { SelectionList += "&nbsp;&nbsp;&nbsp;&nbsp;<a style=\"color: #FF0000\" href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theSection.Section_Title + "</a><br />" + GenerateSelectionList(model); break; }
                            }
                            else { SelectionList += GenerateSelectionList(model); }
                        }
                    }
                    model.Section = "All";
                }
            }
            else if (model.Page.Equals("All"))//Only one unit has been specified
            {
                IQueryable<AnswerApp.Models.Page> retrieved = from theAnswers in db.Pages
                                                              where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                              && theAnswers.Unit_Title.Equals(model.Unit)
                                                              && theAnswers.Chapter_Title.Equals(model.Chapter)
                                                              && theAnswers.Section_Title.Equals(model.Section)
                                                              select theAnswers;
                AnswerApp.Models.Page[] results = retrieved.ToArray<AnswerApp.Models.Page>();
                foreach (Page thePage in results)
                {
                    model.Page = thePage.Page_Number;
                    //newModel.Page = thePage.Page_Number;
                    if (UserHasAccess(User.Identity.Name, model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf"))
                    {
                        SelectionList += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + thePage.Page_Number + "</a><br />" + GenerateSelectionList(model);
                    }
                    else
                    {
                        foreach (String thisAnswer in ThisUsersAnswers)
                        {
                            String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                            if (!(theseProperties.Length < 2))
                            {
                                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                                thisModel.Textbook = theseProperties[0];
                                thisModel.Unit = theseProperties[1];
                                thisModel.Chapter = theseProperties[2];
                                thisModel.Section = theseProperties[3];
                                thisModel.Page = theseProperties[4];
                                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                                if (thisModel.Page.Equals(model.Page)) { SelectionList += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style=\"color: #FF0000\" href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + thePage.Page_Number + "</a><br />" + GenerateSelectionList(model); break; }
                            }
                            else { SelectionList += GenerateSelectionList(model); }
                        }
                    }
                    model.Page = "All";
                }
            }
            else if (model.Question.Equals("All"))//Only one unit has been specified
            {
                IQueryable<AnswerApp.Models.Question> retrieved = from theAnswers in db.Questions
                                                                  where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                                  && theAnswers.Unit_Title.Equals(model.Unit)
                                                                  && theAnswers.Chapter_Title.Equals(model.Chapter)
                                                                  && theAnswers.Section_Title.Equals(model.Section)
                                                                  && theAnswers.Page_Number.Equals(model.Page)
                                                                  select theAnswers;
                AnswerApp.Models.Question[] results = retrieved.ToArray<AnswerApp.Models.Question>();
                foreach (Question theQuestion in results)
                {
                    model.Question = theQuestion.Question_Number;
                    //newModel.Question = theQuestion.Question_Number; 
                    if(UserHasAccess(User.Identity.Name, model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf"))
                    {
                        SelectionList += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theQuestion.Question_Number + "</a><br />" + GenerateSelectionList(model);
                    }
                    else
                    {
                        foreach (String thisAnswer in ThisUsersAnswers)
                        {
                            String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                            if (!(theseProperties.Length < 2))
                            {
                                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                                thisModel.Textbook = theseProperties[0];
                                thisModel.Unit = theseProperties[1];
                                thisModel.Chapter = theseProperties[2];
                                thisModel.Section = theseProperties[3];
                                thisModel.Page = theseProperties[4];
                                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                                if (thisModel.Question.Equals(model.Question)) { SelectionList += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style=\"color: #FF0000\" href=\"Answers/ViewAnswer/" + User.Identity.Name + "?Textbook=" + model.Textbook + "&Unit=" + model.Unit + "&Chapter=" + model.Chapter + "&Section=" + model.Section + "&Page=" + model.Page + "&Question=" + model.Question + "\">" + theQuestion.Question_Number + "</a><br />" + GenerateSelectionList(model); break; }
                            }
                            else { SelectionList += GenerateSelectionList(model); }
                        }
                    }
                    //~/Answers/ViewAnswer/123456?Textbook=Mathematics 10&Unit=All&Chapter=All&Section=All&Page=All&Question=All
                    model.Question = "All";
                }
            }

            return SelectionList;
        }

        //Determines whether a user has access to a given grouping of solutions
        public Boolean UserHasAccess(String UserName, String FileName)
        {
            Boolean UserHasAccess = false;
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.User theUser = new AnswerApp.Models.User();
            theUser = db.Users.Single(u => u.UserName.Equals(User.Identity.Name));

            //Disect the file name for it's file properties
            String[] properties = FileName.Split(new char[1] { '_' });
            AnswerApp.Models.SelectModel model = new AnswerApp.Models.SelectModel();
            model.Textbook = properties[0];
            model.Unit = properties[1];
            model.Chapter = properties[2];
            model.Section = properties[3];
            model.Page = properties[4];
            model.Question = properties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name

            if (theUser.Answers == null) { return false; }

            String[] UserAnswers = theUser.Answers.Split(new char[2] { ',', ';' });
            if (UserAnswers.Length < 2) { return false; }
            foreach (String thisAnswer in UserAnswers)
            {
                if (thisAnswer.Equals(FileName)) { return true; }//They have purchased this exact selection previously
                String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                if (theseProperties.Length < 2) { return false; }
                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                thisModel.Textbook = theseProperties[0];
                thisModel.Unit = theseProperties[1];
                thisModel.Chapter = theseProperties[2];
                thisModel.Section = theseProperties[3];
                thisModel.Page = theseProperties[4];
                thisModel.Question = theseProperties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name

                if (thisModel.Unit.Equals("All") && thisModel.Textbook.Equals(model.Textbook))
                {
                    return true;
                    //FileName = "1";
                }
                else if (thisModel.Chapter.Equals("All") && thisModel.Unit.Equals(model.Unit) && !thisModel.Unit.Equals("All"))
                {
                    return true;
                    //FileName = "2";
                }
                else if (thisModel.Section.Equals("All") && thisModel.Chapter.Equals(model.Chapter) && !thisModel.Chapter.Equals("All"))
                {
                    return true;
                    //FileName = "3";
                }
                else if (thisModel.Page.Equals("All") && thisModel.Section.Equals(model.Section) && !thisModel.Section.Equals("All"))
                {
                    return true;
                    //FileName = "4";
                }
                else if (thisModel.Question.Equals("All") && thisModel.Page.Equals(model.Page) && !thisModel.Page.Equals("All"))
                {
                    return true;
                    //FileName = "5";
                }

            }
            return UserHasAccess;
        }
    }
}
