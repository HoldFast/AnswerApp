using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Text;
using System.IO;

namespace AnswerApp
{
    /// <summary>
    /// Summary description for PaymentConfirmation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PaymentConfirmation : System.Web.Services.WebService
    {
        // This helper method encodes a string correctly for an HTTP POST
        private string Encode(string oldValue)
        {
            string newValue = oldValue.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue);
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        [WebMethod]
        public string HelloWorld()
        {
            //Post back to either sandbox or live
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            string strLive = "https://www.paypal.com/cgi-bin/webscr";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSandbox);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
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
                //check the payment_status is Completed
                //check that txn_id has not been previously processed
                //check that receiver_email is your Primary PayPal email
                //check that payment_amount/payment_currency are correct
                //process payment
            }
            else if (strResponse == "INVALID")
            {
                //log for manual investigation
            }
            else
            {
                //log response/ipn data for manual investigation
            }


            /*// Step 1a: Modify the POST string.
            string formPostData = "cmd = _notify-validate";
            foreach (String postKey in Request.Form)
            {
                string postValue = Encode(Request.Form[postKey]);
                formPostData += string.Format("&{0}={1}", postKey, postValue);
            }

            // Step 1b: POST the data back to PayPal.
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] postByteArray = Encoding.ASCII.GetBytes(formPostData);
            byte[] responseArray = client.UploadData("https://www.paypal.com/cgi-bin/webscr", "POST", postByteArray);
            string response = Encoding.ASCII.GetString(responseArray);

            // Step 1c: Process the response from PayPal.
            switch (response)
            {
                case "VERIFIED":
                    {
                        // Perform steps 2-5 above. 
                        // Continue with automation processing if all steps succeeded.
                        break
                        ;
                    }
                default:
                    {
                        // Possible fraud. Log for investigation.
                        break;
                    }
            } */
            return "Hello World";
        }
    }
}
