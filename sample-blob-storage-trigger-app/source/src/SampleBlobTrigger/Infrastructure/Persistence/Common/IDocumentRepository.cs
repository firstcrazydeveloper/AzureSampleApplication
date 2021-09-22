namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using System;
    using System.Threading.Tasks;
    public interface IDocumentRepository<T>
        where T : class
    {
        Task<T> CreateAsync(T value, string partitionKey);
    }
}
