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
        //String Filename_of_Solution_to_Purchase = null;
        //
        // GET: /Answers/

        public ActionResult Select(string argument, SelectModel model)
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            
            //Populate List of Units
            List<string> TextbookList = new List<string>();
            List<AnswerApp.Models.Textbook> All_Textbooks = new List<AnswerApp.Models.Textbook>();
            All_Textbooks = db.Textbooks.ToList();
            foreach (Textbook theTextbook in All_Textbooks)
            {
                if (theTextbook.Title == model.Textbook)
                {
                    TextbookList.Add(theTextbook.Title);
                }
            }
            
            model.Textbook = argument;
            //List<string> TextbookList = new List<string>();
            //TextbookList.Add(model.Textbook);

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
            
            model.Textbook = Textbook_Title;
            model.Unit = Unit_Title;
            model.Chapter = Chapter_Title;
            model.Section = Section_Title;
            model.Page = Page_Number;
            model.Question = Question_Number;
            
            return RedirectToAction("ViewAnswer/" + User.Identity.Name, "Answers", model);
        }

        public ActionResult ViewAnswer(string argument, SelectModel model)
        {
            ViewData["List"] = "Error: No list";
            ViewData["RenderAnswer"] = "false";//don't render practice answer before the user has answered it

            String FilenameExtensionless = "" + model.Textbook +
                              "_" + model.Unit +
                              "_" + model.Chapter +
                              "_" + model.Section +
                              "_" + model.Page +
                              "_" + model.Question;
            ViewData["FileNameExtensionless"] = FilenameExtensionless;
            String FileName = "" + FilenameExtensionless + ".pdf";
            ViewData["FileName"] = FileName;
            ViewData["PracticeProblemFileName"] = "" + ViewData["FileNameExtensionless"] + "_Practice Problem.png";

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.User theUser = new AnswerApp.Models.User();
            theUser = db.Users.Single(u => u.UserName.Equals(User.Identity.Name));
            if (theUser != null)
            {
                if (theUser.Answers != null)
                {
                    String[] UserAnswers = theUser.Answers.Split(new char[2] { ',', ';' });
                    
                    for (int i = 0; i < UserAnswers.Length; i++)
                    {
                        if (UserHasAccess(User.Identity.Name, FileName))//(UserAnswers[i].Equals(ViewData["FileName"]))//YOU ARE HERE
                        {
                            if (model.Unit.Equals("All") || model.Chapter.Equals("All") || model.Section.Equals("All") || model.Page.Equals("All") || model.Question.Equals("All"))
                            {
                                RedirectToAction("Index", "Home");
                            }            
                            return View("ViewAnswer", model);
                        }
                    }
                }
                return RedirectToAction("Pay", "Answers", model);
            }
            return RedirectToAction("Logon", "Account");
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
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            if (thisUser == null) { RedirectToAction("LogOn", "Account"); }

            String filename = "" + model.Textbook +
                              "_" + model.Unit +
                              "_" + model.Chapter +
                              "_" + model.Section +
                              "_" + model.Page +
                              "_" + model.Question + ".pdf";

            /*TO DO
             * Go through all previous purchases and 
             * erase them from the user's List of Answers:
             * Use String.Replace to finally replace the Purchase_ clause
             * Then split the answer string based on the 
             * replacing string (which must also be a single character) 
             * "*" for example.
             * Save each component of the string so that it can be 
             * reconstructed to the original answer string for that user
             * The frist string in the array will be al answers 
             * before the recent purcahse, the following strings will be
             * all purchases after that one.
             * It is important to note that all of the strings other than the first
             * will be ones that started with purchase meaning they will have the
             * answer that has just been purchased plus all answers already payed for
             * after that purcahase up until the next new purchase.
             * Each of these substrings should be split on the ';' character to isolate 
             * the file name of the solution just purchased.  
             * 
             * Or the easy way is to replace "Purchase_" with "".
             * //*/
            thisUser.Answers += "" + "Purchase_" + filename + ";";//Purchase indicates that the item is not yet payed for.

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
    
            /*TO DO
             * Go through all previous purchases and 
             * erase them from the user's List of Answers:
             * Use String.Replace to finally replace the Purchase_ clause
             * Then split the answer string based on the 
             * replacing string (which must also be a single character) 
             * "*" for example.
             * Save each component of the string so that it can be 
             * reconstructed to the original answer string for that user
             * The frist string in the array will be al answers 
             * before the recent purcahse, the following strings will be
             * all purchases after that one.
             * It is important to note that all of the strings other than the first
             * will be ones that started with purchase meaning they will have the
             * answer that has just been purchased plus all answers already payed for
             * after that purcahase up until the next new purchase.
             * Each of these substrings should be split on the ';' character to isolate 
             * the file name of the solution just purchased.  
             * 
             * Or the easy way is to replace "Purchase_" with "".
             * //*/
            thisUser.Answers += "" + "Purchase_" + filename + ";";//Purchase indicates that the item is not yet payed for.
            //Filename_of_Solution_to_Purchase = "";
            //Filename_of_Solution_to_Purchase = filename;// +";";

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

        //Allows the user to upload new Answers along with their respective
        //Practice Problem and the answer to that Practice Problem
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
                    
                //Disect the file name for it's file properties
                String[] properties = FileName.Split(new char[1] { '_' });
                String Textbook_Title = properties[0];
                String Unit_Title = properties[1];
                String Chapter_Title = properties[2];
                String Section_Title = properties[3];
                String Page_Number = properties[4];
                String Question_Number = properties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                String Practice_Problem = null;
                if (properties.Length > 6) { Practice_Problem = properties[6]; }//An 7th argument indicates a Practice Problem
                if (Practice_Problem != null) { Practice_Problem = properties[6].Split(new char[1] { '.' })[0]; }//Truncate ".pdf" from the end of the file name

                //Search teh database for this Textbook
                IQueryable<Textbook> RetrievedTextbooks = from theTextbooks in db.Textbooks
                                                          where theTextbooks.Title.Equals(Textbook_Title)
                                                          select theTextbooks;
                Textbook[] TextbookResults = RetrievedTextbooks.ToArray<Textbook>();
                if (TextbookResults.Length == 0)//The Textbook does not yet exists
                {
                    //Create a new Textbook
                    AnswerApp.Models.Textbook theTextbook = new AnswerApp.Models.Textbook();

                    //Populate the Textbook with the properties extracted from the file name
                    theTextbook.Title = Textbook_Title;

                    db.Textbooks.InsertOnSubmit(theTextbook);
                    db.SubmitChanges();
                }

                //Search teh database for this Unit
                IQueryable<Unit> RetrievedUnits = from theUnits in db.Units
                                                  where theUnits.Textbook_Title.Equals(Textbook_Title)
                                                  && theUnits.Unit_Title.Equals(Unit_Title)
                                                  select theUnits;
                Unit[] UnitResults = RetrievedUnits.ToArray<Unit>();
                if (UnitResults.Length == 0)//The Unit does not yet exists
                {
                    //Create a new Unit
                    AnswerApp.Models.Unit theUnit = new AnswerApp.Models.Unit();

                    //Populate the Unit with the properties extracted from the file name
                    theUnit.Textbook_Title = Textbook_Title;
                    theUnit.Unit_Title = Unit_Title;
                    //Populate the relational Id's based on previous hierarchical entries
                    theUnit.Textbook_Id = db.Textbooks.Single(d => d.Title.Equals(Textbook_Title)).Unique_Id;

                    db.Units.InsertOnSubmit(theUnit);
                    db.SubmitChanges();
                }

                //Search teh database for this Chapter
                IQueryable<Chapter> RetrievedChapters = from theChapters in db.Chapters
                                                        where theChapters.Textbook_Title.Equals(Textbook_Title)
                                                        && theChapters.Unit_Title.Equals(Unit_Title)
                                                        && theChapters.Chapter_Title.Equals(Chapter_Title)
                                                        select theChapters;
                Chapter[] ChapterResults = RetrievedChapters.ToArray<Chapter>();
                if (ChapterResults.Length == 0)//The Chapter does not yet exists
                {
                    //Create a new Chapter
                    AnswerApp.Models.Chapter theChapter = new AnswerApp.Models.Chapter();

                    //Populate the Chapter with the properties extracted from the file name
                    theChapter.Textbook_Title = Textbook_Title;
                    theChapter.Unit_Title = Unit_Title;
                    theChapter.Chapter_Title = Chapter_Title;
                    //Populate the relational Id's based on previous hierarchical entries
                    theChapter.Textbook_Id = db.Textbooks.Single(d => d.Title.Equals(Textbook_Title)).Unique_Id;
                    theChapter.Unit_Id = db.Units.Single(d => d.Unit_Title.Equals(Unit_Title)).Unit_Id;

                    db.Chapters.InsertOnSubmit(theChapter);
                    db.SubmitChanges();
                }

                //Search teh database for this Section
                IQueryable<Section> RetrievedSections = from theSections in db.Sections
                                                        where theSections.Textbook_Title.Equals(Textbook_Title)
                                                        && theSections.Unit_Title.Equals(Unit_Title)
                                                        && theSections.Chapter_Title.Equals(Chapter_Title)
                                                        && theSections.Section_Title.Equals(Section_Title)
                                                        select theSections;
                Section[] SectionResults = RetrievedSections.ToArray<Section>();
                if (SectionResults.Length == 0)//The Section does not yet exists
                {
                    //Create a new Section
                    AnswerApp.Models.Section theSection = new AnswerApp.Models.Section();

                    //Populate the Section with the properties extracted from the file name
                    theSection.Textbook_Title = Textbook_Title;
                    theSection.Unit_Title = Unit_Title;
                    theSection.Chapter_Title = Chapter_Title;
                    theSection.Section_Title = Section_Title;
                    //Populate the relational Id's based on previous hierarchical entries
                    theSection.Textbook_Id = db.Textbooks.Single(d => d.Title.Equals(Textbook_Title)).Unique_Id;
                    theSection.Unit_Id = db.Units.Single(d => d.Unit_Title.Equals(Unit_Title)).Unit_Id;
                    theSection.Chapter_Id = db.Chapters.Single(d => d.Chapter_Title.Equals(Chapter_Title)).Chapter_Id;

                    db.Sections.InsertOnSubmit(theSection);
                    db.SubmitChanges();
                }

                //Search teh database for this Page
                IQueryable<Page> RetrievedPages = from thePages in db.Pages
                                                  where thePages.Textbook_Title.Equals(Textbook_Title)
                                                  && thePages.Unit_Title.Equals(Unit_Title)
                                                  && thePages.Chapter_Title.Equals(Chapter_Title)
                                                  && thePages.Section_Title.Equals(Section_Title)
                                                  && thePages.Page_Number.Equals(Page_Number)
                                                  select thePages;
                Page[] PageResults = RetrievedPages.ToArray<Page>();
                if (PageResults.Length == 0)//The Page does not yet exists
                {
                    //Create a new Page
                    AnswerApp.Models.Page thePage = new AnswerApp.Models.Page();

                    //Populate the Page with the properties extracted from the file name
                    thePage.Textbook_Title = Textbook_Title;
                    thePage.Unit_Title = Unit_Title;
                    thePage.Chapter_Title = Chapter_Title;
                    thePage.Section_Title = Section_Title;
                    thePage.Page_Number = Page_Number;
                    //Populate the relational Id's based on previous hierarchical entries
                    thePage.Textbook_Id = db.Textbooks.Single(d => d.Title.Equals(Textbook_Title)).Unique_Id;
                    thePage.Unit_Id = db.Units.Single(d => d.Unit_Title.Equals(Unit_Title)).Unit_Id;
                    thePage.Chapter_Id = db.Chapters.Single(d => d.Chapter_Title.Equals(Chapter_Title)).Chapter_Id;
                    thePage.Section_Id = db.Sections.Single(d => d.Section_Title.Equals(Section_Title)).Section_Id;

                    db.Pages.InsertOnSubmit(thePage);
                }

                //Search teh database for this Question
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
                    //Populate the relational Id's based on previous hierarchical entries
                    theQuestion.Textbook_Id = db.Textbooks.Single(d => d.Title.Equals(Textbook_Title)).Unique_Id;
                    theQuestion.Unit_Id = db.Units.Single(d => d.Unit_Title.Equals(Unit_Title)).Unit_Id;
                    theQuestion.Chapter_Id = db.Chapters.Single(d => d.Chapter_Title.Equals(Chapter_Title)).Chapter_Id;
                    theQuestion.Section_Id = db.Sections.Single(d => d.Section_Title.Equals(Section_Title)).Section_Id;
                    theQuestion.Page_Id = db.Pages.Single(d => d.Page_Number.Equals(Page_Number)).Page_Id;

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
                }

                db.SubmitChanges();//Commit the changes to the database.
            }
            return View("Upload", r);
        }

        //When a user requests a file, this method returns the 
        //file as an object in the http response rather than 
        //As a file on the server file system.  
        public ActionResult GetPdf(String argument)
        {
            //Extract the file properties from the file name argument
            String[] arguments = argument.Split(new char[1] { '_' });
            String user = arguments[0];
            String Textbook_Title = arguments[1];
            String Unit_Title = arguments[2];
            String Chapter_Title = arguments[3];
            String Section_Title = arguments[4];
            String Page_Number = arguments[5];
            String Question_Number = arguments[6].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
            String Practice_Problem = null;
            if (arguments.Length > 7){Practice_Problem = arguments[7];}//An 8th argument indicates a Practice Problem
            if (Practice_Problem != null) { Practice_Problem = arguments[7].Split(new char[1] { '.' })[0]; }//Truncate ".pdf" from the end of the file name

            //If the username is not the one specified in the file name, redirect them
            if (!User.Identity.Name.Equals(user)) { return RedirectToAction("LogOn", "Account"); }

            String filename = argument;//The name of the file will be prefixed by the username of the person retrieving it

            //Generate the file name under which the answer will be stored in the database
            String FileNameInDB = "" + arguments[1] +
                                  "_" + arguments[2] +
                                  "_" + arguments[3] +
                                  "_" + arguments[4] +
                                  "_" + arguments[5] +
                                  "_" + arguments[6];

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            
            //Check to see that the user is registered
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            
            //If the user is not in the database they will be redirected to the registration page
            if (thisUser == null) { RedirectToAction("Register", "Account"); }

            //If the user has previous answers then check them to see if this is one of them
            if (thisUser.Answers != null)
            {
                String[] UserAnswers = thisUser.Answers.Split(new char[2] { ',', ';' });

                //Check to see if the user already has that answer
                //for (int index = 0; index < UserAnswers.Length; index++)//For each answer in the user's answers
                {
                    if (UserHasAccess(User.Identity.Name, FileNameInDB))//If this answer is the answer we're looking for//YOU ARE HERE
                    {
                        //Retrieve the answer from the database
                        IQueryable<Question> retrieved = from theAnswers in db.Questions
                                                         where theAnswers.Textbook_Title.Equals(Textbook_Title)
                                                         && theAnswers.Unit_Title.Equals(Unit_Title)
                                                         && theAnswers.Chapter_Title.Equals(Chapter_Title)
                                                         && theAnswers.Section_Title.Equals(Section_Title)
                                                         && theAnswers.Page_Number.Equals(Page_Number)
                                                         && theAnswers.Question_Number.Equals(Question_Number)
                                                         select theAnswers;
                        Question[] results = retrieved.ToArray<Question>();
                        if (results.Length == 0) { return RedirectToAction("ResourceUnavailable", "Home"); }//If the answer doesn't exist in the database then redirect them
                        AnswerApp.Models.Question theQuestion = results.First();
                        byte[] pdfBytes = null;
                        if (Practice_Problem != null)//This is a Practice Problem
                        {
                            pdfBytes = theQuestion.Practice_Problem.ToArray();
                        }
                        else//(Practice_Problem == null) This is not a Practice Problem
                        {
                            pdfBytes = theQuestion.Answer.ToArray(); 
                        }
                        return new PdfResult(pdfBytes, false, filename);
                    }//else this answer is not the answer we're loking for so continue searching
                }
            }
            //After checking all of the users answers, if this Answer is not listed, redirect to select page
            return RedirectToAction("Select", "Answers");
        }

        public ActionResult PayPal(String argument)//Chage this name to long string of alphanumerics to prevent access
        {
            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();
            AnswerApp.Models.User thisUser = db.Users.Single(d => d.UserName.Equals(User.Identity.Name));
            if (thisUser == null) { return RedirectToAction("LogOn", "Account"); }

            String thisUsersAnswers = thisUser.Answers.Replace("Purchase_", "*");// += Filename_of_Solution_to_Purchase +";";

            String[] Solution_Just_Purchased = thisUsersAnswers.Split(new char[1] { '*' });
            String[] Local_Filename_of_Solution_to_Purchase = Solution_Just_Purchased[1].Split(new char[1] { ';' });

            thisUser.Answers = thisUser.Answers.Replace("Purchase_", "");

            db.SubmitChanges();

            //Disect the file name for it's file properties
            String[] properties = Local_Filename_of_Solution_to_Purchase[0].Split(new char[1] { '_' });
            String Textbook_Title = properties[0];
            String Unit_Title = properties[1];
            String Chapter_Title = properties[2];
            String Section_Title = properties[3];
            String Page_Number = properties[4];
            String Question_Number = properties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
            String Practice_Problem = null;
            if (properties.Length > 6) { Practice_Problem = properties[6]; }//An 7th argument indicates a Practice Problem
            if (Practice_Problem != null) { Practice_Problem = properties[6].Split(new char[1] { '.' })[0]; }//Truncate ".pdf" from the end of the file name//*/

            AnswerApp.Models.SelectModel model = new AnswerApp.Models.SelectModel();
            model.Textbook = Textbook_Title;
            model.Unit = Unit_Title;
            model.Chapter = Chapter_Title;
            model.Section = Section_Title;
            model.Page = Page_Number;
            model.Question = Question_Number;

            return RedirectToAction("ViewAnswer/" + User.Identity.Name, "Answers", model);
        }

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

            foreach(String thisAnswer in UserAnswers)
            {
                if (thisAnswer.Equals(FileName)) { return true; }//They have purchased this exact selection previously
                String[] theseProperties = thisAnswer.Split(new char[1] { '_' });
                AnswerApp.Models.SelectModel thisModel = new AnswerApp.Models.SelectModel();
                thisModel.Textbook = properties[0];
                thisModel.Unit = properties[1];
                thisModel.Chapter = properties[2];
                thisModel.Section = properties[3];
                thisModel.Page = properties[4];
                thisModel.Question = properties[5].Split(new char[1] { '.' })[0];//Truncate ".pdf" from the end of the file name
                //return thisModel.Textbook.Equals(model.Textbook) &&;
                
                //if (thisAnswer.Equals(FileName))//YOU ARE HERE
                {
                    if (thisModel.Unit.Equals("All") && thisModel.Textbook.Equals(model.Textbook))
                    {
                        return true;
                    }
                    else if (thisModel.Chapter.Equals("All") && thisModel.Unit.Equals(model.Unit))
                    {
                        return true;
                    }
                    else if (thisModel.Section.Equals("All") && thisModel.Chapter.Equals(model.Chapter))
                    {
                        return true;
                    }
                    else if (thisModel.Page.Equals("All") && thisModel.Section.Equals(model.Section))
                    {
                        return true;
                    }
                    else if (thisModel.Question.Equals("All") && thisModel.Page.Equals(model.Page))
                    {
                        return true;
                    }
                    else if (thisModel.Question.Equals(model.Question))
                    {
                        return true;
                    }
                }
            }
            return UserHasAccess;
        }
    }
}
