﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapCardBalance.WebApi.Model
{
    public class BalanceResponse
    {
        public string CardNo { get; set; }

        public string Balance { get; set; }

        public bool Finished { get; set; }

        public bool Error { get; set; }

        public string TimeLasted { get; internal set; }
    }
}
