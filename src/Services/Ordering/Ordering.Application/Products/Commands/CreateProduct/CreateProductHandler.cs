using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Products.Commands.CreateProduct;
public class CreateProductHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productId = command.Product.Id != Guid.Empty ? command.Product.Id : Guid.NewGuid();

        var product = Product.Create(
            id: ProductId.Of(productId),
            name: command.Product.Name,
            price: command.Product.Price
        );

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id.Value);
    }
}
