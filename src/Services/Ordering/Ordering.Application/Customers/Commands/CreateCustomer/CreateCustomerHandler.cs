using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateCustomerCommand, CreateCustomerResult>
{
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customerId = command.Customer.Id != Guid.Empty ? command.Customer.Id : Guid.NewGuid();

        var customer = Customer.Create(
            id: CustomerId.Of(customerId),
            name: command.Customer.Name,
            email: command.Customer.Email
        );

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCustomerResult(customer.Id.Value);
    }
}