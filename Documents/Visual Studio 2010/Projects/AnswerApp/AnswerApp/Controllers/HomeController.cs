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
                
                





                
                
                //else
                //{

                    //Calculate the percentages of each textbook the user has purchased
                    /*List<String> UserTextbooks = new List<String>(); ;
                    List<String> UserAnswers = thisUser.Answers.Split(new char[1] { ';' }).ToList();
                    foreach (String theAnswer in UserAnswers)
                    {
                        String[] AnswerProperties = theAnswer.Split(new char[1] { '_' });//AnswerProperties[0] is theTextbook
                        if (!UserTextbooks.Contains(AnswerProperties[0]))
                        {
                            UserTextbooks.Add(AnswerProperties[0]);//If it is not already in the list of textbooks add it
                            String[] AnswerPercentage = theAnswer.Split(new char[1] { ',' });
                            Textbook thisTextbook = new Textbook();
                            thisTextbook.TextbookTitle = AnswerProperties[0];
                            thisTextbook.TextbookPercentage = AnswerPercentage[1];
                            model.UserTextbooks.Add(thisTextbook);
                        }
                        
                    }//*/
                //}

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
    }
}
