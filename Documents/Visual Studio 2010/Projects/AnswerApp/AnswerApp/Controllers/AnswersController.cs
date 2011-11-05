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
            ViewData["RenderAnswer"] = "false";//don't render practice answer before the user has answered it

            ViewData["FileNameExtensionless"] = "" + model.Textbook +
                                                    "_" + model.Unit +
                                                    "_" + model.Chapter +
                                                    "_" + model.Section +
                                                    "_" + model.Page +
                                                    "_" + model.Question;
            ViewData["FileName"] = "" + ViewData["FileNameExtensionless"] + ".pdf";
            ViewData["PracticeProblemFileName"] = "" + ViewData["FileNameExtensionless"] + "_Practice Problem.png";

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User theUser = new AnswerApp.Models.User();
            theUser = db.Users.Single(u => u.UserName.Equals(User.Identity.Name));
            if (theUser != null)
            {
                if (theUser.Answers != null)
                {
                    string[] UserAnswers = new string[100];
                    UserAnswers = theUser.Answers.Split(new char[2] { ',', ';' });
                    
                    for (int i = 0; i < UserAnswers.Length; i++)
                    {
                        if (UserAnswers[i].Equals(ViewData["FileName"]))
                        {
                            return View("ViewAnswer", model);
                        }
                    }
                }
            }
            return RedirectToAction("Pay", "Answers", model);
        }

        [HttpPost]
        public ActionResult ViewAnswer(string argument, SelectModel model, string returnUrl)
        {
            ViewData["RenderAnswer"] = "true";//The answer will be rendered since this is a post back
            ViewData["PracticeProblemAnswer"] = model.PracticeProblemAnswer;//populate the correct answer
            
            ViewData["FileNameExtensionless"] = "" + model.Textbook +
                                                    "_" + model.Unit +
                                                    "_" + model.Chapter +
                                                    "_" + model.Section +
                                                    "_" + model.Page +
                                                    "_" + model.Question;
            ViewData["FileName"] = "" + ViewData["FileNameExtensionless"] + ".pdf";
            ViewData["PracticeProblemFileName"] = "" + ViewData["FileNameExtensionless"] + "_Practice Problem.png";

            //Retrieve the practice problem answer
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
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
            return View("ViewAnswer", model);
        }

        public ActionResult Pay(SelectModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Pay(SelectModel model, string returnUrl)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            if (thisUser == null) { RedirectToAction("LogOn", "Account"); }

            String filename = "" + model.Textbook +
                              "_" + model.Unit +
                              "_" + model.Chapter +
                              "_" + model.Section +
                              "_" + model.Page +
                              "_" + model.Question + ".pdf";

            thisUser.Answers += filename + ";";

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
            return RedirectToAction("ViewAnswer/purchase", "Answers", model);
        }

        //INCOMPLETE
        public ActionResult Upload(UploadModel model, string returnUrl)
        {
            List<ViewDataUploadFilesResult> r = new List<ViewDataUploadFilesResult>();

            HttpPostedFileBase hpf = null;// = Request.Files[file] as HttpPostedFileBase;
            String FileName = null;

            foreach (string file in Request.Files)
            {
                hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                //string savedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\" + Path.GetFileName(hpf.FileName));
                string savedFileName = Path.Combine(Path.GetFileName(hpf.FileName));
                //hpf.SaveAs(savedFileName);//Replace this with database insertion
                FileName = Path.GetFileName(hpf.FileName);// hpf.FileName;

                r.Add(new ViewDataUploadFilesResult()
                {
                    Name = savedFileName,
                    Length = hpf.ContentLength
                });

                AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
                    
                //if (hpf != null)
                {
                    //AnswerApp.Models.Question theQuestion = db.Questions.Single<Question>(d => d.Question_Id == 2);
                    
                    //Disect the file name for it's file properties
                    String[] properties = FileName.Split(new char[1] { '_' });
                    String Textbook_Title = properties[0];
                    String Unit_Title = properties[1];
                    String Chapter_Title = properties[2];
                    String Section_Title = properties[3];
                    String Page_Number = properties[4];
                    String Question_Number = properties[5].Split(new char[1] { '.' })[0];
                    String Practice_Problem = null;
                    //Practice_Problem = properties[7];
                    if (properties.Length > 6) { Practice_Problem = properties[6]; }
                    if (Practice_Problem != null) { Practice_Problem = properties[6].Split(new char[1] { '.' })[0]; }

                    IQueryable<Question> retrieved = from theAnswers in db.Questions
                                                     where theAnswers.Textbook_Title.Equals(Textbook_Title)
                                                     && theAnswers.Unit_Title.Equals(Unit_Title)
                                                     && theAnswers.Chapter_Title.Equals(Chapter_Title)
                                                     && theAnswers.Section_Title.Equals(Section_Title)
                                                     && theAnswers.Page_Number.Equals(Page_Number)
                                                     && theAnswers.Question_Number.Equals(Question_Number)
                                                     select theAnswers;
                    Question[] results = retrieved.ToArray<Question>();
                    if (results.Length != 0)//The Answer already exists
                    {
                        //Use the existing Question
                        AnswerApp.Models.Question theQuestion = results.First();

                        if (Practice_Problem != null)//This is a Practice Problem
                        {
                            theQuestion.Practice_Problem = new BinaryReader(hpf.InputStream).ReadBytes((int)hpf.InputStream.Length);
                            theQuestion.Practice_Problem_Answer = model.PracticeProblemAnswer;
                        }
                        else//(Practice_Problem == null) This is an Answer
                        {
                            theQuestion.Answer = new BinaryReader(hpf.InputStream).ReadBytes((int)hpf.InputStream.Length);
                            theQuestion.Practice_Problem_Answer = model.PracticeProblemAnswer;
                        }
                    }
                    else//(results.Length == 0) This is a new Answer
                    {
                        //Create a new Question
                        AnswerApp.Models.Question theQuestion = new AnswerApp.Models.Question();

                        //Populate the Question with the properties extracted from the file name
                        theQuestion.Textbook_Title = Textbook_Title;
                        theQuestion.Unit_Title = Unit_Title;
                        theQuestion.Chapter_Title = Chapter_Title;
                        theQuestion.Section_Title = Section_Title;
                        theQuestion.Page_Number = Page_Number;
                        theQuestion.Question_Number = Question_Number;

                        //db.Questions.InsertOnSubmit(theQuestion);
                        //db.SubmitChanges();
                        //AnswerApp.Models.Question retrieved = db.Questions.Single<Question>(d => d.Question_Id == theQuestion.Question_Id);
                        //theQuestion = retrieved;

                        if (Practice_Problem != null)//This is a Practice Problem
                        {
                            theQuestion.Practice_Problem = new BinaryReader(hpf.InputStream).ReadBytes((int)hpf.InputStream.Length);
                            theQuestion.Practice_Problem_Answer = model.PracticeProblemAnswer;
                        }
                        else//(Practice_Problem == null) This is an Answer
                        {
                            theQuestion.Answer = new BinaryReader(hpf.InputStream).ReadBytes((int)hpf.InputStream.Length);
                            theQuestion.Practice_Problem_Answer = model.PracticeProblemAnswer;
                        }

                        //Insert the new Question into the database
                        db.Questions.InsertOnSubmit(theQuestion);

                        //ViewData["Test"] = Practice_Problem;
                    }
                }

                db.SubmitChanges();//Commit the changes to the database.
                
            }
            //Question retrieved = db.Questions.Single(d => d.Question_Id == theQuestion.Question_Id);
            //ViewBag.RetrievedAnswer = theQuestion.ToString();// retrieved.ToString();
            //ViewBag.RetrievedAnswer = theQuestion.Question_Id;// retrieved.Question_Id;

            return View("Upload", r);
        }

        //When a user requests a file, this method returns the 
        //file as an object in the http response rather than 
        //As a file on the server file system.  
        public ActionResult GetPdf(String argument)
        {

            //String[] arguments = new String[8];
            String[] arguments = argument.Split(new char[1] { '_' });
            String user = arguments[0];
            String Textbook_Title = arguments[1];
            String Unit_Title = arguments[2];
            String Chapter_Title = arguments[3];
            String Section_Title = arguments[4];
            String Page_Number = arguments[5];
            String Question_Number = arguments[6].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
            String Practice_Problem = null;
            if (arguments.Length == 8){Practice_Problem = arguments[7];}
            if (Practice_Problem != null){Practice_Problem = arguments[7].Split(new char[1] { '.' })[0];}

            if (!User.Identity.Name.Equals(user)) { return RedirectToAction("LogOn", "Account"); }

            String filename = argument;//The name of the file will be prefixed by the username of the person retrieving it

            String FileNameInDB = "" + arguments[1] +
                                  "_" + arguments[2] +
                                  "_" + arguments[3] +
                                  "_" + arguments[4] +
                                  "_" + arguments[5] +
                                  "_" + arguments[6];

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            
            AnswerApp.Models.Question theQuestion = new AnswerApp.Models.Question();

            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            
            if (thisUser == null) { RedirectToAction("LogOn", "Account"); }

            //If the user has previous answers then check them to see if this is one of them
            if (thisUser.Answers != null)
            {
                string[] UserAnswers = new string[100];
                UserAnswers = thisUser.Answers.Split(new char[2] { ',', ';' });

                //Check to see if the user already has that answer (This will only be necesary for when a user uses the back button to reach the purchase page again.
                for (int index = 0; index < UserAnswers.Length; index++)
                {
                    if (UserAnswers[index].Equals(FileNameInDB))
                    {
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
                        theQuestion = results.First();
                        byte[] pdfBytes = null;
                        if (Practice_Problem != null)//.Equals("Practice Problem"))
                        {
                            pdfBytes = theQuestion.Practice_Problem.ToArray();
                        }
                        else
                        {
                            pdfBytes = theQuestion.Answer.ToArray(); 
                        }
                        return new PdfResult(pdfBytes, false, filename);
                    }
                }
            }
            return RedirectToAction("Pay", "Answers");
        }
    }
}
