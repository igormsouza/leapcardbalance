﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapCardBalance.WebApi.Model
{
    public class BalanceRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
