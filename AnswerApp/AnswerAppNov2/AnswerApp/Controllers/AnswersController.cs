using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnswerApp.Models;
using System.IO;
using System.Net.Mime;

namespace AnswerApp.Controllers
{
    /// <summary>
    /// Returns the proper Response Headers and "Content-Disposition" for a PDF file,
    /// and allows you to specify the filename and whether it will be downloaded by the browser.
    /// </summary>
    public class PdfResult : FileContentResult
    {
        public ContentDisposition ContentDisposition { get; private set; }

        /// <summary>
        /// Returns a PDF FileResult.
        /// </summary>
        /// <param name="pdfFileContents">The data for the PDF file</param>
        /// <param name="download">Determines if the file should be shown in the browser or downloaded as a file</param>
        /// <param name="filename">The filename that will be shown if the file is downloaded or saved.</param>
        /// <param name="filenameArgs">A list of arguments to be formatted into the filename.</param>
        /// <returns></returns>
        //[JetBrains.Annotations.StringFormatMethod("filename")]
        public PdfResult(byte[] pdfFileContents, bool download, string filename, params object[] filenameArgs)
            : base(pdfFileContents, "application/pdf")
        {
            // Format the filename:
            if (filenameArgs != null && filenameArgs.Length > 0)
            {
                filename = string.Format(filename, filenameArgs);
            }

            // Add the filename to the Content-Disposition
            ContentDisposition = new ContentDisposition
                                     {
                                         Inline = !download,
                                         FileName = filename,
                                         Size = pdfFileContents.Length,
                                     };
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            // Add the filename to the Content-Disposition
            response.AddHeader("Content-Disposition", ContentDisposition.ToString());
            base.WriteFile(response);
        }
    }//*/

    public class AnswersController : Controller
    {
        //
        // GET: /Answers/

        public ActionResult Select(string argument, SelectModel model)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            model.Textbook = argument;
            List<string> TextbookList = new List<string>();
            TextbookList.Add(model.Textbook);

            //Populate List of Units
            List<string> UnitList = new List<string>();
            List<AnswerApp.Models.Unit> All_Units = new List<AnswerApp.Models.Unit>();
            All_Units = db.Units.ToList();
            foreach (Unit theUnit in All_Units)
            {
                if (theUnit.Textbook_Title == model.Textbook)
                {
                    UnitList.Add(theUnit.Unit_Title);
                }
            }

            //Populate List of Chapters
            List<string> ChapterList = new List<string>();
            List<AnswerApp.Models.Chapter> All_Chapters = new List<AnswerApp.Models.Chapter>();
            All_Chapters = db.Chapters.ToList();
            foreach (Chapter theChapter in All_Chapters)
            {
                if (theChapter.Textbook_Title == model.Textbook)
                {
                    ChapterList.Add(theChapter.Chapter_Title);
                }
            }

            //Populate List of Sections
            List<string> SectionList = new List<string>();
            List<AnswerApp.Models.Section> All_Sections = new List<AnswerApp.Models.Section>();
            All_Sections = db.Sections.ToList();
            foreach (Section theSection in All_Sections)
            {
                if (theSection.Textbook_Title == model.Textbook)
                {
                    SectionList.Add(theSection.Section_Title);
                }
            }

            //Populate List of Pages
            List<string> PageList = new List<string>();
            List<AnswerApp.Models.Page> All_Pages = new List<AnswerApp.Models.Page>();
            All_Pages = db.Pages.ToList();
            foreach (Page thePage in All_Pages)
            {
                if (thePage.Textbook_Title == model.Textbook)
                {
                    PageList.Add(thePage.Page_Number);
                }
            }

            //Populate List of Questions
            List<string> QuestionList = new List<string>();
            List<AnswerApp.Models.Question> All_Questions = new List<AnswerApp.Models.Question>();
            All_Questions = db.Questions.ToList();
            foreach (Question thequestion in All_Questions)
            {
                if (thequestion.Question_Number != null)//This filters out test data
                {
                        QuestionList.Add(thequestion.Question_Number);
                }
            }

            ViewBag.TextbookDropDownList = new SelectList(TextbookList);
            ViewBag.UnitDropDownList = new SelectList(UnitList);
            ViewBag.ChapterDropDownList = new SelectList(ChapterList);
            ViewBag.SectionDropDownList = new SelectList(SectionList);
            ViewBag.PageDropDownList = new SelectList(PageList);
            ViewBag.QuestionDropDownList = new SelectList(QuestionList);

            return View(model);
        }

        [HttpPost]
        public ActionResult Select(SelectModel model, string returnUrl)
        {
            return RedirectToAction("ViewAnswer/" + User.Identity.Name, "Answers", model);
        }

        public ActionResult ViewAnswer(string argument, SelectModel model)
        {
            ViewData["RenderAnswer"] = "false";
            if (User.Identity.Name.Equals(argument))
            {
                ViewData["FileName"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf";
                ViewData["FileNameExtensionless"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question;
                ViewData["PracticeProblemFileName"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + "_Practice Problem.png";

                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
                AnswerApp.Models.User theUser = new AnswerApp.Models.User();
                theUser = db.Users.Single(u => u.UserName.Equals(User.Identity.Name));
                if (theUser != null)
                {
                    if (theUser.Answers != null)
                    {
                        string[] UserAnswers = new string[100];
                        UserAnswers = theUser.Answers.Split(new char[2] { ',', ';' });
                        ViewData["AnswerString"] = theUser.Answers;
                        ViewData["UserName"] = theUser.UserName;
                        ViewData["UserIdentity"] = User.Identity.Name;

                        for (int i = 0; i < UserAnswers.Length; i++)
                        {
                            if (UserAnswers[i].Equals(ViewData["FileName"]))
                            {
                                return View("ViewAnswer", model/*, r*/);
                            }
                        }
                    }
                }
                return RedirectToAction("Pay", "Answers", model);//pay here!!!!
            }
            else
            {
                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
                AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
                if (thisUser == null) { RedirectToAction("LogOn", "Account"); }//This should never happen

                //If the user has previous answers then check them to see if this is one of them
                if (thisUser.Answers != null)
                {
                    string[] UserAnswers = new string[100];
                    UserAnswers = thisUser.Answers.Split(new char[2] { ',', ';' });
                    ViewData["UserAnswers"] = UserAnswers[0];
                    ViewData["AnswerString"] = thisUser.Answers;
                    ViewData["UserName"] = thisUser.UserName;
                    ViewData["UserIdentity"] = User.Identity.Name;

                    //Check to see if the user already has that answer (THis will only be necesary for when a user uses the back button to reach the purchase page again.
                    for (int index = 0; index < UserAnswers.Length; index++)
                    {
                        if (UserAnswers[index].Equals(ViewData["FileName"]))
                        {
                            return View("ViewAnswer", model/*, r*/);//If the user already has the answer then show it.
                        }
                    }
                }
                
                //Once it is certain that the user doesn't have this answer then it will be added to their list of answers.
                ViewData["FileName"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf";
                ViewData["FileNameExtensionless"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question;
                ViewData["PracticeProblemFileName"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + "_Practice Problem.png";
                thisUser.Answers += ViewData["FileName"] + ",0%;";
                db.SubmitChanges();

                return View("ViewAnswer", model/*, r*/);
            }
        }

        [HttpPost]
        public ActionResult ViewAnswer(string argument, SelectModel model, string returnUrl)
        {
            ViewData["PracticeProblemAnswer"] = model.PracticeProblemAnswer;
            ViewData["RenderAnswer"] = "true";
            if (User.Identity.Name.Equals(argument))
            {
                ViewData["FileName"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf";
                ViewData["FileNameExtensionless"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question;
                ViewData["PracticeProblemFileName"] = "" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + "_Practice Problem.png";

                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
                AnswerApp.Models.User theUser = new AnswerApp.Models.User();
                theUser = db.Users.Single(u => u.UserName.Equals(User.Identity.Name));
                if (theUser != null)
                {
                    if (theUser.Answers != null)
                    {
                        string[] UserAnswers = new string[100];
                        UserAnswers = theUser.Answers.Split(new char[2] { ',', ';' });
                        ViewData["AnswerString"] = theUser.Answers;
                        ViewData["UserName"] = theUser.UserName;
                        ViewData["UserIdentity"] = User.Identity.Name;

                        for (int i = 0; i < UserAnswers.Length; i++)
                        {
                            if (UserAnswers[i].Equals(ViewData["FileName"]))
                            {
                                ////String PracticeProblemAnswer = db.ExecuteQuery("SELECT Practice_Problem_Answer FROM Question WHERE Textbook_Title = {0}", model.Textbook);
                                model.CorrectAnswer = "Error 1";//<br />" + model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question;
                                IQueryable<Question> retrieved = from theAnswers in db.Questions
                                                                 where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                                 && theAnswers.Unit_Title.Equals(model.Unit)
                                                                 && theAnswers.Chapter_Title.Equals(model.Chapter)
                                                                 && theAnswers.Section_Title.Equals(model.Section)
                                                                 && theAnswers.Page_Number.Equals(model.Page)
                                                                 && theAnswers.Question_Number.Equals(model.Question)
                                                                 select theAnswers;
                                Question[] results = retrieved.ToArray<Question>();
                                if (results.Length != 0)
                                {
                                    model.CorrectAnswer = results.First().Practice_Problem_Answer;
                                }
                                db.SubmitChanges();
                                return View("ViewAnswer", model);
                            }
                        }
                    }
                }
                return RedirectToAction("Pay", "Answers", model);//pay here!!!!
            }
            else//This user just purchased the answer
            {
                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
                AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
                if (thisUser == null) { RedirectToAction("LogOn", "Account"); }//This should never happen

                //If the user has previous answers then check them to see if this is one of them
                if (thisUser.Answers != null)
                {
                    string[] UserAnswers = new string[100];
                    UserAnswers = thisUser.Answers.Split(new char[2] { ',', ';' });
                    ViewData["UserAnswers"] = UserAnswers[0];
                    ViewData["AnswerString"] = thisUser.Answers;
                    ViewData["UserName"] = thisUser.UserName;
                    ViewData["UserIdentity"] = User.Identity.Name;

                    //Check to see if the user already has that answer (This will only be necesary for when a user uses the back button to reach the purchase page again.
                    for (int index = 0; index < UserAnswers.Length; index++)
                    {
                        if (UserAnswers[index].Equals(ViewData["FileName"]))
                        {
                            model.CorrectAnswer = "Error 2";
                            IQueryable<Question> retrieved = from theAnswers in db.Questions
                                                             where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                             && theAnswers.Unit_Title.Equals(model.Unit)
                                                             && theAnswers.Chapter_Title.Equals(model.Chapter)
                                                             && theAnswers.Section_Title.Equals(model.Section)
                                                             && theAnswers.Page_Number.Equals(model.Page)
                                                             && theAnswers.Question_Number.Equals(model.Question)
                                                             select theAnswers;
                            Question[] results = retrieved.ToArray<Question>();
                            if (results.Length != 0)
                            {
                                model.CorrectAnswer = results.First().Practice_Problem_Answer;
                            }
                            db.SubmitChanges();
                            return View("ViewAnswer", model/*, r*/);//If the user already has the answer then show it.
                        }
                    }
                }

                //Once it is certain that the user doesn't have this answer then it will be added to their list of answers.
                ViewData["FileName"] = model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + ".pdf";
                ViewData["FileNameExtensionless"] = model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question;
                ViewData["PracticeProblemFileName"] = model.Textbook + "_" + model.Unit + "_" + model.Chapter + "_" + model.Section + "_" + model.Page + "_" + model.Question + "_Practice Problem.png";
                thisUser.Answers += ViewData["FileName"] + ",0%;";

                model.CorrectAnswer = "Error 3";
                IQueryable<Question> retrieved2 = from theAnswers in db.Questions
                                                 where theAnswers.Textbook_Title.Equals(model.Textbook)
                                                 && theAnswers.Unit_Title.Equals(model.Unit)
                                                 && theAnswers.Chapter_Title.Equals(model.Chapter)
                                                 && theAnswers.Section_Title.Equals(model.Section)
                                                 && theAnswers.Page_Number.Equals(model.Page)
                                                 && theAnswers.Question_Number.Equals(model.Question)
                                                 select theAnswers;
                Question[] results2 = retrieved2.ToArray<Question>();
                if (results2.Length != 0)
                {
                    model.CorrectAnswer = results2.First().Practice_Problem_Answer; 
                }
                db.SubmitChanges();
                return View("ViewAnswer", model/*, r*/);
            }
        }

        public ActionResult Pay(SelectModel model)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            if (thisUser == null) { RedirectToAction("LogOn", "Account"); }//This should never happen

            //thisUser.Answers += "," + model.FileName;

            db.SubmitChanges();

            return View(model);//RedirectToAction("ViewAnswer/" + User.Identity.Name, "Answers");
        }

        [HttpPost]
        public ActionResult Pay(SelectModel model, string returnUrl)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            if (thisUser == null) { RedirectToAction("LogOn", "Account"); }//This should never happen

            //thisUser.Answers += "," + model.FileName;

            List<Question> All_PracticeProblems = db.Questions.ToList();
            foreach (Question thisPracticeProblem in All_PracticeProblems)
            {
                if ((thisPracticeProblem.Textbook_Title == model.Textbook) && (thisPracticeProblem.Unit_Title == model.Unit) && (thisPracticeProblem.Chapter_Title == model.Chapter) && (thisPracticeProblem.Section_Title == model.Section) && (thisPracticeProblem.Page_Number == model.Page) && (thisPracticeProblem.Question_Number == model.Question))
                {
                    model.CorrectAnswer = thisPracticeProblem.Practice_Problem_Answer;
                }
            }
            db.SubmitChanges();
            return RedirectToAction("ViewAnswer/purchase", "Answers", model);
        }

        //INCOMPLETE
        public ActionResult Upload(UploadModel model, string returnUrl)
        {
            List<ViewDataUploadFilesResult> r = new List<ViewDataUploadFilesResult>();

            HttpPostedFileBase hpf = null;// = Request.Files[file] as HttpPostedFileBase;

            foreach (string file in Request.Files)
            {
                hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                string savedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\" + Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName);//Replace this with database insertion

                r.Add(new ViewDataUploadFilesResult()
                {
                    Name = savedFileName,
                    Length = hpf.ContentLength
                });
            }
            ViewData["FileName"] = "This is a file name.";

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.Question theQuestion = new AnswerApp.Models.Question(); //db.Questions.Single(d => d.Question_Number.Equals("8"));//

            if (hpf != null)
            {
                theQuestion.Answer = new BinaryReader(hpf.InputStream).ReadBytes((int)hpf.InputStream.Length);
                //db.Questions.InsertOnSubmit(theQuestion);
                
            }
            db.SubmitChanges();


            Question retrieved = db.Questions.Single(d => d.Question_Id == theQuestion.Question_Id);
            ViewBag.RetrievedAnswer = retrieved.ToString();
            ViewBag.RetrievedAnswer = retrieved.Question_Id;


            return View("Upload", r);
        }

        public ActionResult GetPdf(String argument)
        {

            String[] arguments = new String[7];
            arguments = argument.Split(new char[1] { '_' });
            String user = arguments[0];
            String Textbook_Title = arguments[1];
            String Unit_Title = arguments[2];
            String Chapter_Title = arguments[3];
            String Section_Title = arguments[4];
            String Page_Number = arguments[5];
            String Question_Number = arguments[6].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name

            if (!User.Identity.Name.Equals(user)) { return RedirectToAction("LogOn", "Account"); }

            String filename = argument;//The name of the file will be prefixed by the username of the person retrieving it

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            
            AnswerApp.Models.Question theQuestion = new AnswerApp.Models.Question();

            IQueryable<Question> retrieved = from theAnswers in db.Questions
                                 where theAnswers.Textbook_Title.Equals(Textbook_Title)
                                 && theAnswers.Unit_Title.Equals(Unit_Title)
                                 && theAnswers.Chapter_Title.Equals(Chapter_Title)
                                 && theAnswers.Section_Title.Equals(Section_Title)
                                 && theAnswers.Page_Number.Equals(Page_Number)
                                 && theAnswers.Question_Number.Equals(Question_Number)
                                 select theAnswers;
            Question[] results = retrieved.ToArray<Question>();
            if (results.Length == 0) { return RedirectToAction("ResourceUnavailable", "Home"); }
            theQuestion = results.First();// Single<Question>(q => q.Question_Number.Equals(Question_Number));
            byte[] pdfBytes = theQuestion.Answer.ToArray();
            return new PdfResult(pdfBytes, false, filename);
        }
    }
}
