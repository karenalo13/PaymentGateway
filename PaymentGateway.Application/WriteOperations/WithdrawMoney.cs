﻿using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
   public class WithdrawMoney : IWriteOperation<MakeWithdraw>
    {
        public IEventSender eventSender;
        private readonly Database _database;

        public WithdrawMoney(IEventSender eventSender, Database database)
        {
            this.eventSender = eventSender;
            _database = database;
        }

        public void PerformOperation(MakeWithdraw operation)
        {
            var account = _database.BankAccounts.FirstOrDefault(acc => acc.Iban == operation.Iban);
            if(account== null)
            {
                throw new Exception("invalid account");

            }

            var user = _database.Persons.FirstOrDefault(pers => pers.Cnp == operation.Cnp );
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if(user.Accounts.FindIndex(r => r.Iban == account.Iban) == -1)
            {
                throw new Exception("invalid attempt");
            }
            if (account.Limit < operation.Amount)
            {
                throw new Exception("cannot withdraw this amount");
            }
            if (account.Balance < operation.Amount)
            {
                throw new Exception("insufficient funds");
            }
            var transaction = new Transaction();
            transaction.Amount = operation.Amount;
            transaction.Currency = account.Currency;
            transaction.Date = DateTime.UtcNow;
            transaction.Type = "Withdraw";
            account.Balance -= operation.Amount;
            _database.SaveChanges();

            WithdrawMade wm = new WithdrawMade();
            wm.Amount = operation.Amount;
            wm.Iban = operation.Iban;
            eventSender.SendEvent(wm);



        }
    }
}



