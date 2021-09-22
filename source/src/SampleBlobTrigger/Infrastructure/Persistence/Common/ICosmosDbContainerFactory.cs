namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ICosmosDbContainerFactory
    {
        ICosmosDbContainer GetContainer(string containerName);
    }
}
