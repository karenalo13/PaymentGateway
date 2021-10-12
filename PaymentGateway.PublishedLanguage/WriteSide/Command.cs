﻿using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
     public class Command
    {
        public List<CommandDetails> Details { get; set; } = new List<CommandDetails>();
        public string Cnp { get; set; }
        public string Iban { get; set; }
    }
}
