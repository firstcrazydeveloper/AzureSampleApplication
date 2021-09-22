namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using Microsoft.Azure.Cosmos;
    public interface ICosmosDbContainer
    {
        Container DbContainer { get; }
    }
}
