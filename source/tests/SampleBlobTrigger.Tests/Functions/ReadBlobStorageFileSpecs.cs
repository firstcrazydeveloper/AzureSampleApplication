namespace SampleBlobTrigger.Tests.Functions
{
    using FunctionAppBlobStorageTrigger.Functions;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SampleBlobTrigger.Application;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class ReadBlobStorageFileSpecs
    {
        private ReadBlobStorageFileFunction _readBlobStorageFileFunction;
        private Mock<ICandidateInsightService> _candidateInsightServiceMock;
        private Mock<ILogger<ReadBlobStorageFileFunction>> _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerMock = new Mock<ILogger<ReadBlobStorageFileFunction>>();
            _candidateInsightServiceMock = new Mock<ICandidateInsightService>();
            _readBlobStorageFileFunction = new ReadBlobStorageFileFunction(_candidateInsightServiceMock.Object, _loggerMock.Object);
        }

        [TestClass]
        public class ReadBlobStorageFileAsync : ReadBlobStorageFileSpecs
        {

            [TestMethod]
            public async Task WhenFileAdded_ThenTriggerNotifyEvent()
            {
                _candidateInsightServiceMock.Setup(x => x.InsertDataToCosmosAsync(It.IsAny<Stream>(), It.IsAny<string>())).Returns(Task.FromResult(true));

                using (var testStream = new MemoryStream(Encoding.UTF8.GetBytes("TestData")))
                {
                    
                    // Act
                    await _readBlobStorageFileFunction.ReadFileFromBlobStorage(testStream, "sampletrigger", "testfile").ConfigureAwait(false);

                    // Assert
                    _candidateInsightServiceMock.Verify(x => x.InsertDataToCosmosAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
                }
            }

        }
    }
}
