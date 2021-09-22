namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IDocumentRepository<T>
        where T : class
    {
        Task CreateAsync(T value, string partitionKey);

        Task<bool> InsertBulkData(IDictionary<string, T> values, IDictionary<string, string> partitionKeys);
    }
}
