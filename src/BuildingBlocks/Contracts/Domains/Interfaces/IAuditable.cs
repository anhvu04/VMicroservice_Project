namespace Contracts.Domains.Interfaces;

public interface IAuditable : IUserTracking, IDateTracking, ISoftDelete
{
}