using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeapCardRobot
{
    public class LeapCardFlow
    {
        public WebBrowser wb;
        public bool islogin;
        public DataLeapCard Request { get; set; }

        public LeapCardFlow(DataLeapCard dataLeapCard)
        {
            Request = dataLeapCard;
        }

        public void GetBalance()
        {
            DataLeapCard result = Request;

            wb.Navigate("https://www.leapcard.ie/en/login.aspx");
            wb.ScriptErrorsSuppressed = true;
            wb.DocumentCompleted += wb_DocumentCompleted;
        }

        public void GetBalanceQuick()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var dataToPost = Encoding.Default.GetBytes("ctl00$ContentPlaceHolder1$UserName=igorafff&ctl00$ContentPlaceHolder1$Password=M0reiraa");
                    var result = client.UploadData("https://www.leapcard.ie/en/login.aspx", "POST", dataToPost);
                    string documentText = Encoding.Default.GetString(result);
                    AnaliseData(documentText);
                }
            }
            catch (Exception ex)
            {
                Request.Error = true;
                Request.OnCompleted();
            }
        }

        public void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wb.Url.ToString().IndexOf("login.aspx") > -1 && !islogin)
                {
                    wb.Document.GetElementById("ContentPlaceHolder1_UserName").InnerText = Request.Login;
                    wb.Document.GetElementById("ContentPlaceHolder1_Password").InnerText = Request.Password;
                    wb.Document.GetElementById("ContentPlaceHolder1_btnlogin").InvokeMember("click");
                    islogin = true;
                }
                else if (wb.Url.ToString().IndexOf("CardOverView.aspx") > -1)
                {
                    string documentText = wb.DocumentText;
                    AnaliseData(documentText);
                }
                else if (islogin)
                {
                    Request.Error = true;
                    Request.OnCompleted();
                }
            }
            catch (Exception)
            {
                Request.Error = true;
                Request.OnCompleted();
            }
        }

        public void AnaliseData(string documentText)
        {
            var split = Regex.Split(documentText, "Travel Credit Balance")[1];

            split = split.Substring(0, split.IndexOf("<div style="));
            split = split.Substring(split.IndexOf("pull-left"));
            split = split.Substring(split.IndexOf(">") + 1);
            split = split.Substring(0, split.IndexOf("<"));

            Request.Balance = split;
            Request.OnCompleted();
        }
    }
}
