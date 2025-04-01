using Contracts.Common.Interfaces;
using Product.Repositories.Entities;
using Product.Repositories.GenericRepository;
using Product.Repositories.Persistence;

namespace Product.Repositories.UnitOfWork;

public interface IProductUnitOfWork : IUnitOfWork<ProductContext>
{
    IProductGenericRepository<CatalogProduct, Guid> CatalogProducts { get; }
}