using System;
using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class AccountMade: INotification
    {
        public String Name { get; set; }
        
    }
}
