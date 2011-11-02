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
        public ActionResult Index(HomeModel model)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Message = "The Answer App";
                //String Username = User.Identity.Name;
                ViewBag.Username = User.Identity.Name;//Username;
                
                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));

                if (thisUser == null) { RedirectToAction("LogOn", "Account"); }//This should never happen
                else
                {
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
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account"); //return View();
            }
        }

        public ActionResult ResourceUnavailable(HomeModel model)
        {
            return View();
        }
    }
}
