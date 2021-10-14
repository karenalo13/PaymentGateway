﻿using Abstractions;
using MediatR;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    public class PurchaseProduct : IRequestHandler<Command>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;

        public PurchaseProduct(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();

            BankAccount account = _database.BankAccounts.FirstOrDefault(x => x.Iban == request.Iban);

            if (account == null)
            {
                throw new Exception("Invalid Account");
            }
            double total = 0;
            foreach (var item in request.Details)
            {
                Product product = _database.Products.FirstOrDefault(x => x.ID == item.idProd);

                if (product.Limit < item.Quantity)
                {
                    throw new Exception("Product not in stock");
                }
                total += product.Value * item.Quantity;

                if (account.Balance < total)
                {
                    throw new Exception("Insufficient funds");
                }

                ProductXTransaction pxt = new ProductXTransaction
                {
                    IdProduct = product.ID,
                    IdTransaction = transaction.ID,
                    Quantity = item.Quantity
                };
                product.Limit -= item.Quantity;


                _database.ProductXTransaction.Add(pxt);
            }


            _database.SaveChanges();

            return Unit.Task;
        }
    }
}
