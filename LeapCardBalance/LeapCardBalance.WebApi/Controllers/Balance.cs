using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeapCardBalance.WebApi.Model;
using System.Windows.Forms;
using System.Diagnostics;

namespace LeapCardBalance.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class Balance : Controller
    {
        [HttpGet]
        public BalanceResponse Get()
        {
            return new BalanceResponse() { Balance = "ok" };
        }

        LeapCardFlow lcf;
        WebBrowser wb;
        DataLeapCard request;
        BalanceResponse result = null;

        // http://localhost:59977/api/balance/getBalance?login={login}&password={password}
        [HttpGet("getBalance")]
        public async Task<BalanceResponse> GetBalance(string login, string password)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            result = new BalanceResponse();

            request = new DataLeapCard(login, password);
            request.EvDataLeapCardCompleted += request_EvDataLeapCardCompleted;

            lcf = new LeapCardFlow(request);
            lcf.wb = this.wb;

            await lcf.GetBalanceAsync();
            result.Finished = true;

            stopwatch.Stop();
            result.TimeLasted = string.Format("{0:hh\\:mm\\:ss}", stopwatch.Elapsed);
            return result;
        }

        void request_EvDataLeapCardCompleted(object sender, EvDataLeapCardCompleted e)
        {
            if (e.Error)
            {
                result.Error = true;
                result.ErrorMessage = e.ErrorMessage;
                //MessageBox.Show("Error", "Error");
                // Todo Log
            }
            else
            {
                result.Balance = e.Balance;
                result.CardNo = e.CardNo;
                //MessageBox.Show("Your Balance is:" + e.Balance, "Balance");
                // Todo Log
            }

            result.Steps = e.Steps;
        }
    }
}
