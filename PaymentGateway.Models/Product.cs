using System;

namespace PaymentGateway.Models
{
    public class Product
    {
        public int ID  { get; set; } 
        public string Name { get; set; }
        public double Value { get; set; }

        public String Currency { get; set; }

        public int Limit { get; set; }


    }
}
