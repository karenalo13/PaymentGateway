using System;

namespace PaymentGateway.Models
{
    public class Product
    {
        public int Id  { get; set; } 
        public string Name { get; set; }
        public decimal Value { get; set; }

        public String Currency { get; set; }

        public int Limit { get; set; }


    }
}
