using Contracts.Common.Interfaces;
using Product.Domain.Entities;
using Product.Domain.GenericRepository;

namespace Product.Domain.UnitOfWork;

public interface IProductUnitOfWork : IUnitOfWork
{
    IProductGenericRepository<CatalogProduct, Guid> CatalogProduct { get; }
}