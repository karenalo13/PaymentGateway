﻿using Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Application.WriteOperations;
using static PaymentGateway.Application.Queries.ListOfAccounts;

namespace PaymentGateway.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<EnrollCustomerOperation>();
            services.AddTransient<CreateAccount>();
            services.AddTransient<DepositMoney>();
            services.AddTransient<WithdrawMoney>();
            services.AddTransient<PurchaseProduct>();
            services.AddSingleton<NewIban>();
            services.AddSingleton<Data.Database>();

            services.AddTransient<IValidator<Query>, ListOfAccounts.Validator>();
            services.AddTransient<QueryHandler>();

            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var options = new AccountOptions
                {
                    InitialBalance = config.GetValue("AccountOptions:InitialBalance", 0)
                };
                return options;
            });


            return services;
        }
    }
}
