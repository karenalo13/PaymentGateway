using Abstractions;
using MediatR;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using PaymentGateway.PublishedLanguage.Events;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.CommandHandlers
{
    public class PurchaseProduct : IRequestHandler<PurchaseCommand>
    {
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _dbContext;

        public PurchaseProduct(IMediator mediator, PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(PurchaseCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();

            BankAccount account = _dbContext.BankAccounts.FirstOrDefault(x => x.Iban == request.Iban);

            if (account == null)
            {
                throw new Exception("Invalid Account");
            }
            decimal total = 0;
            foreach (var item in request.Details)
            {
                Product product = _dbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);

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
                    IdProduct = product.Id,
                    IdTransaction = transaction.ID,
                    Quantity = item.Quantity
                };
                product.Limit -= item.Quantity;


                _dbContext.ProductXTransaction.Add(pxt);
            }

            var productPurschased = new ProductPurschased();
            productPurschased.CommandDetails = request.Details;
            _dbContext.SaveChanges();
            await _mediator.Publish(productPurschased, cancellationToken);
            return Unit.Value;
        }
    }
}
