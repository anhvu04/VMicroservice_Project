using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Contracts.Common.Interfaces.MediatR;

namespace Product.Application.Usecases.CatalogProduct.Command.UpdateCatalogProduct;

public class UpdateCatalogProductCommand : ICommand
{
    [JsonIgnore] public Guid Id { get; set; }
    public string? Name { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
    public int? OriginalPrice { get; set; }
    public int? SalePrice { get; set; }

    public string? Thumbnail { get; set; }
}