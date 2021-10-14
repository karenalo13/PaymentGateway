﻿using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.PublishedLanguage.Commands;
using PaymentGateway.Application.ReadOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace PaymentGateway.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MediatR.IMediator _mediator;
        public AccountController(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<string> CreateAccount(MakeNewAccount command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return "OK";
        }

        [HttpGet]
        [Route("ListOfAccounts")]
        public async Task<List<ListOfAccounts.Model>> GetListOfAccounts([FromQuery] ListOfAccounts.Query query, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(query, cancellationToken);

            return result;
        }
    }
}
