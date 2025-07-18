using ScheduledJob.Domain.Entities;
using ScheduledJob.Domain.GenericRepository;
using Infrastructure.Common.Implementation;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.ConfigurationSettings;

namespace ScheduledJob.Persistence.GenericRepository;

public class JobLogsRepository : MongoDbRepository<JobLogs>, IJobLogsRepository
{
    public JobLogsRepository(IMongoClient client, IOptions<DatabaseSettings> settings) : base(client, settings)
    {
    }
}