namespace FunctionAppBlobStorageTrigger
{
    using System;

    public class FunctionAppConfiguration : IFunctionAppConfiguration
    {
        public string BlobTrigerName => throw new NotImplementedException();

        public string BlobConnectionString => throw new NotImplementedException();
    }
}
