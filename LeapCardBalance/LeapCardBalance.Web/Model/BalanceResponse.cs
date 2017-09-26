using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapCardBalance.Web.Model
{
    public class BalanceResponse
    {
        public string Balance { get; set; }

        public bool Finished { get; set; }

        public bool Error { get; set; }
    }
}
