using Contracts.Common.Interfaces;
using ScheduledJob.Domain.Entities;

namespace ScheduledJob.Domain.GenericRepository;

public interface IJobLogsRepository : IMongoDbRepository<JobLogs>
{
    
}