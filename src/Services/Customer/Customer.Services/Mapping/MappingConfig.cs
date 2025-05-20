using Customer.Repositories.Entities;
using Customer.Services.Models.Requests.CustomerSegment;
using Customer.Services.Models.Responses.CustomerSegment;
using Infrastructure.Extensions.MappingExtensions;
using Mapster;

namespace Customer.Services.Mapping;

public class MappingConfig
{
    public static void RegisterConfig()
    {
        TypeAdapterConfig<UpdateCustomerSegmentRequest, CustomerSegment>.NewConfig().IgnoreNullProperties();
    }
}