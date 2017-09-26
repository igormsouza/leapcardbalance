using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeapCardBalance.Web.Model;

namespace LeapCardBalance.Web.Controllers
{
    [Route("api/[controller]")]
    public class Balance : Controller
    {
        [HttpGet]
        public BalanceResponse Get1()
        {
            return new BalanceResponse() { Balance = "1", Finished = true };
        }

        LeapCardFlow lcf;
        WebBrowser wb;
        DataLeapCard request;

        // http://localhost:59977/api/balance/byName?login=igorafff&password=M0reiraa
        [HttpGet("byName")]
        public BalanceResponse Get(string login, string password)
        {
            return new BalanceResponse() { Balance = login + " " + password, Finished = true };

            request = new DataLeapCard("igorafff", "M0reiraa");
            request.EvDataLeapCardCompleted += request_EvDataLeapCardCompleted;

            lcf = new LeapCardFlow(request);
            lcf.wb = this.webBrowser1;
            lcf.GetBalance();
        }

        void request_EvDataLeapCardCompleted(object sender, EvDataLeapCardCompleted e)
        {
            if (e.Error)
            {
                //MessageBox.Show("Error", "Error");
            }
            else
            {
                //MessageBox.Show("Your Balance is:" + e.Balance, "Balance");
            }
        }
    }
}
