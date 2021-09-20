namespace FunctionAppBlobStorageTrigger
{
    public interface IFunctionAppConfiguration
    {
        string BlobTrigerName { get; }
        string BlobConnectionString { get; }
    }
}
