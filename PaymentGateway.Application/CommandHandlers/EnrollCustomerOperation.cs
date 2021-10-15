using Abstractions;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using System;
using MediatR;
using System.Threading.Tasks;
using PaymentGateway.PublishedLanguage.Commands;
using System.Threading;
using PaymentGateway.PublishedLanguage.Events;

namespace PaymentGateway.Application.CommandHandlers
{
    public class EnrollCustomerOperation : IRequestHandler<EnrollCustomer>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        private readonly NewIban _ibanService;
        public EnrollCustomerOperation(IMediator mediator, Database database, NewIban ibanService)
        {
            _mediator = mediator;
            _database = database;
            _ibanService = ibanService;
        }

        public async Task<Unit> Handle(EnrollCustomer request, CancellationToken cancellationToken)
        {
            var customer = new Person
            {
                Cnp = request.UniqueIdentifier,
                Name = request.Name
            };
            if (request.ClientType == "Company")
            {
                customer.TypeOfPerson = PersonType.Company;
            }

            else if (request.ClientType == "Individual")
            {
                customer.TypeOfPerson = PersonType.Individual;
            }
            else
            {
                throw new Exception("Unsupported person type");
            }
            customer.Id = _database.Persons.Count + 1;
            _database.Persons.Add(customer);

            var account = new BankAccount
            {
                Type = request.AccountType,
                Currency = request.Valuta,
                Balance = 0,
                Iban = _ibanService.GetNewIban()
            };

            _database.BankAccounts.Add(account);

            _database.SaveChanges();

            var ec = new CustomerEnrolled
            {
                Name = customer.Name,
                UniqueIdentifier = customer.Cnp,
                ClientType = request.ClientType
            };
            await _mediator.Publish(ec, cancellationToken);
            return Unit.Value;
        }
    }
}