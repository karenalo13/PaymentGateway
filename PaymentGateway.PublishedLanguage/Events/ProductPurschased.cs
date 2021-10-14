using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
   public class ProductPurschased
    {
        public List<CommandDetails> CommandDetails = new List<CommandDetails>();
    }
}
