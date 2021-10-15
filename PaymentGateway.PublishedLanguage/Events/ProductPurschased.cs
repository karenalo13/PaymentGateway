using MediatR;
using PaymentGateway.Models;
using System.Collections.Generic;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ProductPurschased : INotification
    {
        public List<CommandDetails> CommandDetails = new List<CommandDetails>();
    }
}
