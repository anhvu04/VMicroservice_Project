using System.ComponentModel.DataAnnotations;
using Contracts.Common.Interfaces.MediatR;

namespace Product.Application.Usecases.CatalogProduct.Command.CreateCatalogProduct;

public class CreateCatalogProductCommand : ICommand
{
    public required string Name { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
    public required int OriginalPrice { get; set; }

    public int SalePrice { get; set; }

    public string? Thumbnail { get; set; }
}