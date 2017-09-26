using LeapCardBalance.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeapCardBalance.WebApi
{
    public class LeapCardFlow
    {
        public string firstPage = "https://www.leapcard.ie/en/SelfServices/CardServices/CardOverView.aspx";
        public WebBrowser wb;
        public bool islogin;

        public DataLeapCard Request { get; set; }

        public LeapCardFlow(DataLeapCard dataLeapCard)
        {
            Request = dataLeapCard;
            Request.Steps = 1;
        }

        public async Task GetBalanceAsync()
        {
             DataLeapCard result = Request;

            await Task.Run(() =>
            {
                var t = new Thread(MyThreadStartMethod);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            })
            .ContinueWith((task) =>
            {
                while (!Request.Finished)
                {
                    Thread.Sleep(1000);
                }
            });
        }

        private void MyThreadStartMethod()
        {
            Request.Steps = 2;
            if (wb == null)
            {
                wb = new WebBrowser();
                wb.DocumentCompleted += wb_DocumentCompleted;
                wb.ScriptErrorsSuppressed = true;
                wb.Navigate(firstPage);
                Application.Run();
            }
        }

        public void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                Request.Steps = 3;
                if (wb.Url.ToString().IndexOf("login.aspx") > -1 && !islogin)
                {
                    Request.Steps = 4;
                    wb.Document.GetElementById("ContentPlaceHolder1_UserName").InnerText = Request.Login;
                    wb.Document.GetElementById("ContentPlaceHolder1_Password").InnerText = Request.Password;
                    wb.Document.GetElementById("ContentPlaceHolder1_btnlogin").InvokeMember("click");
                    islogin = true;
                }
                else if (wb.Url.ToString().IndexOf("CardOverView.aspx") > -1)
                {
                    Request.Steps = 5;
                    string documentText = wb.DocumentText;
                    AnaliseData(documentText);
                    Request.OnCompleted();
                    Request.Finished = true;
                }
                else if (islogin)
                {
                    Request.Error = true;
                    Request.ErrorMessage = "Login error.";
                    Request.OnCompleted();
                    Request.Finished = true;
                }
            }
            catch (Exception ex)
            {
                Request.Error = true;
                Request.ErrorMessage = ex.Message;
                Request.OnCompleted();
                Request.Finished = true;
            }
        }

        public void AnaliseData(string documentText)
        {
            Request.Steps = 6;
            var splitBalance = Regex.Split(documentText, "Travel Credit Balance")[1];

            splitBalance = splitBalance.Substring(0, splitBalance.IndexOf("<div style="));
            splitBalance = splitBalance.Substring(splitBalance.IndexOf("pull-left"));
            splitBalance = splitBalance.Substring(splitBalance.IndexOf(">") + 1);
            splitBalance = splitBalance.Substring(0, splitBalance.IndexOf("<"));

            var splitNo = Regex.Split(documentText, "Card Number</label>")[1];
            splitNo = splitNo.Substring(splitNo.IndexOf("col-xs-4") + 11);
            splitNo = splitNo.Substring(0, splitNo.IndexOf("<")).Trim();

            Request.Balance = splitBalance;
            Request.CardNo = splitNo;
            Request.Steps = 7;
            Request.OnCompleted();
        }
    }

    //public void GetBalanceQuick()
    //{
    //    try
    //    {
    //        //using (var client = new WebClient())
    //        //{
    //        //    var dataToPost = Encoding.Default.GetBytes("ctl00$ContentPlaceHolder1$UserName=igorafff&ctl00$ContentPlaceHolder1$Password=M0reiraa");
    //        //    var result = client.UploadData("https://www.leapcard.ie/en/login.aspx", "POST", dataToPost);
    //        //    string documentText = Encoding.Default.GetString(result);
    //        //    AnaliseData(documentText);
    //        //}

    //        WebRequest request = WebRequest.Create("https://www.leapcard.ie/en/login.aspx");
    //        // Set the Method property of the request to POST.  
    //        request.Method = "POST";
    //        // Create POST data and convert it to a byte array.  
    //        string postData = "ctl00$ContentPlaceHolder1$UserName=igorafff&ctl00$ContentPlaceHolder1$Password=M0reiraa";
    //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
    //        // Set the ContentType property of the WebRequest.  
    //        request.ContentType = "application/x-www-form-urlencoded";
    //        // Set the ContentLength property of the WebRequest.  
    //        request.ContentLength = byteArray.Length;
    //        // Get the request stream.  
    //        Stream dataStream = request.GetRequestStream();
    //        // Write the data to the request stream.  
    //        dataStream.Write(byteArray, 0, byteArray.Length);
    //        // Close the Stream object.  
    //        dataStream.Close();
    //        // Get the response.  
    //        WebResponse response = request.GetResponse();
    //        // Display the status.  
    //        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
    //        // Get the stream containing content returned by the server.  
    //        dataStream = response.GetResponseStream();
    //        // Open the stream using a StreamReader for easy access.  
    //        StreamReader reader = new StreamReader(dataStream);
    //        // Read the content.  
    //        string responseFromServer = reader.ReadToEnd();
    //        // Display the content.  
    //        Console.WriteLine(responseFromServer);
    //        // Clean up the streams.  
    //        reader.Close();
    //        dataStream.Close();
    //        response.Close();

    //        AnaliseData(responseFromServer);
    //        //using (var client = new WebClient())
    //        //{
    //        //    var values = new NameValueCollection();
    //        //    values["UserName"] = "igorafff";
    //        //    values["Password"] = "M0reiraa";

    //        //    var response = client.UploadValues("https://www.leapcard.ie/en/login.aspx", values);

    //        //    var responseString = Encoding.Default.GetString(response);
    //        //    AnaliseData(responseString);
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        Request.Error = true;
    //        Request.OnCompleted();
    //    }
    //}
}
