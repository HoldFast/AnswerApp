using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.Page;

using System.Net;
using System.Text;
using System.IO;

using System.Web.Mvc;
using AnswerApp.Models;
using System.IO;
using System.Net.Mime;


namespace AnswerApp
{
    /// <summary>
    /// Summary description for PaymentConfirmation
    /// </summary>
    [WebService(Namespace = "http://solvation.ca/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class PaymentConfirmation : System.Web.Services.WebService
    {
        // This helper method encodes a string correctly for an HTTP POST
        public string Encode(string oldValue)
        {
            string newValue = oldValue.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue);
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        [WebMethod]
        public string Test()
        {
            string argument = "n/a";

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

            administrator.MetaData += "(in) ";

            //db.SubmitChanges();


            //Post back to either sandbox or live
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            string strLive = "https://www.paypal.com/cgi-bin/webscr";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strLive);//Sandbox);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);//byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);

            //String strparam = param.ToString();
            //String splitstrparam = strparam.Split(new char[1] { '&' })[1];
            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
            //req.Proxy = proxy;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            if (strResponse == "VERIFIED")
            {
                String[] parameters = strRequest.Split(new char[1] { '&' });
                //List<String> ParameterList = parameters.ToList<String>();
                String theParameterProxy = "error 03148";
                String[] KeyValue = new String[2];
                String Key = "", Value = "";
                foreach (String theParameter in parameters)
                {
                    KeyValue = theParameter.Split(new char[1] { '=' });
                    Key = KeyValue[0];
                    Value = KeyValue[1];
                    if (Key.Equals("transaction_subject"))
                    {
                        administrator.MetaData += " argument: " + Value;//YOU ARE HERE!!!
                        //theParameterProxy = Value.Replace('+', ' ');
                        db.SubmitChanges();


                        String Properties = Value.Replace('+', ' ');
                        String UserName = Properties.Split(new char[1] { '_' })[0];
                        String FileName = Properties.Replace(UserName + "_", "");

                        AnswerApp.Models.User theUser = db.Users.Single<User>(u => u.UserName.Equals(UserName));
                        theUser.Answers += FileName + ".pdf;";
                        theParameterProxy = UserName + "hello" + FileName + ".pdf;";
                    }
                }
                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += " request: " + strRequest + " response: " + strResponse + " if: " + theParameterProxy;

                db.SubmitChanges();
                //check the payment_status is Completed
                //check that txn_id has not been previously processed
                //check that receiver_email is your Primary PayPal email
                //check that payment_amount/payment_currency are correct
                //process payment
            }
            else if (strResponse == "INVALID")
            {
                //log for manual investigation

                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += " request: " + strRequest + " response: " + strResponse + " else if: " + argument;

                db.SubmitChanges();
            }
            else
            {
                //log response/ipn data for manual investigation

                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += " request: " + strRequest + " response: " + strResponse + " else: " + argument;

                db.SubmitChanges();
            }

            //administrator.MetaData += "request: " + strRequest + " response: " + strResponse + " argument: " + argument;

            //db.SubmitChanges();

            return strResponse;
        }//*/

        [WebMethod]
        public string Test2()
        {
            string argument = "n/a";

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

            administrator.MetaData += "(in) ";

            //db.SubmitChanges();


            //Post back to either sandbox or live
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            string strLive = "https://www.paypal.com/cgi-bin/webscr";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strLive);//Sandbox);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);//byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            
            //String strparam = param.ToString();
            //String splitstrparam = strparam.Split(new char[1] { '&' })[1];
            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
            //req.Proxy = proxy;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            if (strResponse == "VERIFIED")
            {
                String[] parameters = strRequest.Split(new char[1] { '&' });
                //List<String> ParameterList = parameters.ToList<String>();
                String[] KeyValue = new String[2];
                String Key = "", Value = "";
                foreach (String theParameter in parameters)
                {
                    KeyValue = theParameter.Split(new char[1] { '=' });
                    Key = KeyValue[0];
                    Value = KeyValue[1];
                    if (Key.Equals("txn_id"))
                    {
                        AnswerApp.Models.Transaction theTransaction = new AnswerApp.Models.Transaction();
                        theTransaction.Transaction_ID = Value;
                        db.Transactions.InsertOnSubmit(theTransaction);
                        //db.SubmitChanges();
                        administrator.MetaData += " argument: " + Value;
                    }
                }
                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += " request: " + strRequest + " response: " + strResponse + " if: " + strRequest;

                db.SubmitChanges();
                //check the payment_status is Completed
                //check that txn_id has not been previously processed
                //check that receiver_email is your Primary PayPal email
                //check that payment_amount/payment_currency are correct
                //process payment
            }
            else if (strResponse == "INVALID")
            {
                //log for manual investigation

                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += " request: " + strRequest + " response: " + strResponse + " else if: " + argument;

                db.SubmitChanges();
            }
            else
            {
                //log response/ipn data for manual investigation

                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += " request: " + strRequest + " response: " + strResponse + " else: " + argument;

                db.SubmitChanges();
            }

            //administrator.MetaData += "request: " + strRequest + " response: " + strResponse + " argument: " + argument;

            //db.SubmitChanges();

            return strResponse;
        }//*/

        
        
        /*[WebMethod]
        public string Test(string argument)
        {
            //string argument = "nothing";

            AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

            AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

            administrator.MetaData += ",tested ";

            db.SubmitChanges();


            //Post back to either sandbox or live
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            string strLive = "https://www.paypal.com/cgi-bin/webscr";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strLive);//Sandbox);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);//byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            //String strparam = param.ToString();
            //String splitstrparam = strparam.Split(new char[1] { '&' })[1];
            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
            //req.Proxy = proxy;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            if (strResponse == "VERIFIED")
            {
                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += "request: " + strRequest + "response: " + strResponse + "," + argument;

                db.SubmitChanges();
                //check the payment_status is Completed
                //check that txn_id has not been previously processed
                //check that receiver_email is your Primary PayPal email
                //check that payment_amount/payment_currency are correct
                //process payment
            }
            else if (strResponse == "INVALID")
            {
                //log for manual investigation

                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += "request: " + strRequest + "response: " + strResponse + "," + argument;

                db.SubmitChanges();
            }
            else
            {
                //log response/ipn data for manual investigation

                //AnswerApp.Models.AnswerAppDataContext db = new AnswerApp.Models.AnswerAppDataContext();

                //AnswerApp.Models.User administrator = db.Users.Single<AnswerApp.Models.User>(a => a.UserName.Equals("administrator"));

                administrator.MetaData += "request: " + strRequest + "response: " + strResponse + "," + argument;

                db.SubmitChanges();
            }


            //administrator.MetaData += "request: " + strRequest + "response: " + strResponse + "," + argument;

            //db.SubmitChanges();

            return strResponse;
        }//*/

    }
}
