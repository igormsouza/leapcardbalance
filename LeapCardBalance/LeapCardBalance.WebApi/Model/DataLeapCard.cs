using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapCardBalance.WebApi.Model
{
    public class DataLeapCard
    {
        public DataLeapCard(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }

        public event EventHandler<EvDataLeapCardCompleted> EvDataLeapCardCompleted;

        public string Login { get; set; }

        public string Password { get; set; }

        public string CardNo { get; set; }

        public string Balance { get; set; }

        public bool Finished { get; set; }

        public bool Error { get; set; }

        public void OnCompleted()
        {
            var parameter = new EvDataLeapCardCompleted(Balance, CardNo, Error);

            if (EvDataLeapCardCompleted != null)
                EvDataLeapCardCompleted(this, parameter);
        }
    }

    public class EvDataLeapCardCompleted : EventArgs
    {
        public string Balance { get; set; }

        public string CardNo { get; set; }

        public bool Error { get; set; }

        private EvDataLeapCardCompleted() { }

        public EvDataLeapCardCompleted(string balance, string cardNo, bool error)
        {
            Balance = balance;
            CardNo = cardNo;
            Error = error;
        }
    }
}
