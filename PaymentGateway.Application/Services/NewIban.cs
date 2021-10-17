using PaymentGateway.Data;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.Application.ReadOperations
{
    public class NewIban
    {
        private readonly PaymentDbContext _dbContext;

        public NewIban(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetNewIban()
        {
            List<string> ibans = _dbContext.BankAccounts.Select(x => x.Iban).ToList();
           
            if (ibans.Count == 0)
                return "1";
            
            return (_dbContext.BankAccounts.Count()+1).ToString();
            //return (int.Parse(ibans.Last()) + 1).ToString();
        }
    }
}
