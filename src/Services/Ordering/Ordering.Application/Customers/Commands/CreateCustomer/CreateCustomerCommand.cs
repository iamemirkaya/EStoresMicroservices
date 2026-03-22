using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;


namespace Ordering.Application.Customers.Commands.CreateCustomer;


public record CreateCustomerCommand(CustomerDto Customer) : ICommand<CreateCustomerResult>;

public record CreateCustomerResult(Guid Id);

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Customer.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Customer.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required");
    }
}