using CDR;
using CDR.Services;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace CDRTests
{
    public class CallServiceTests
    {
        private IFileSystem _fileSystem;
        private readonly string _header = "caller_id,recipient,call_date,end_time,duration,cost,reference,currency";

        [SetUp]
        public void Setup()
        {
            _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                    @"c:\example_empty.csv", new MockFileData("")
                },
                {
                    @"c:\example_only_header.csv", new MockFileData(_header )
                },
                {
                    @"c:\example.csv", new MockFileData(_header + "\n" +
                                                        "123456789000,98765432111,01/01/2024,00:00:00,567,5.56,C5DA9724701EEBBA95CA2CC5617BA93E4,GBP"+"\n"+
                                                        "98765432111,123456789000,31/12/2024,23:23:59,244,0,C50B5A7BDB8D68B8512BB14A9D363CAA1,GBP"
                    )
                },
                {
                    @"c:\example_incorrect_date.csv", new MockFileData(_header + "\n" +
                                                        "123456789000,98765432111,31/02/2024,00:00:00,567,5.56,C5DA9724701EEBBA95CA2CC5617BA93E4,GBP"
                    )
                }
            } );
        }

        [Test]
        public async Task UploadFile_Empty_NoAddedRows()
        {
            // Arrange
            var context = new AppDbContext();
            var service = new CallService(context);
            var count = context.CallDetails.Count();
            using var stream = _fileSystem.File.OpenRead(@"c:\example_empty.csv");

            // Act
            var added_count = await service.AddDetailsFromFile(stream);
            var newcount = context.CallDetails.Count();
            
            // Assert
            Assert.Zero(added_count);
            Assert.AreEqual(count, newcount);
        }

        [Test]
        public async Task UploadFile_OnlyHeader_NoAddedRows()
        {
            // Arrange
            var context = new AppDbContext();
            var service = new CallService(context);
            var count = context.CallDetails.Count();
            using var stream = _fileSystem.File.OpenRead(@"c:\example_only_header.csv");

            // Act
            var added_count = await service.AddDetailsFromFile(stream);
            var newcount = context.CallDetails.Count();

            // Assert
            Assert.Zero(added_count);
            Assert.AreEqual(count, newcount);
        }

        [Test]
        public async Task UploadFile_FileWithData_TwoAddedRows()
        {
            // Arrange
            var context = new AppDbContext();
            var service = new CallService(context);
            var count = context.CallDetails.Count();
            using var stream = _fileSystem.File.OpenRead(@"c:\example.csv");

            // Act
            var added_count = await service.AddDetailsFromFile(stream);
            var newcount = context.CallDetails.Count();

            Assert.AreEqual(2, added_count);
            Assert.AreEqual(2, newcount-count);
        }

        [Test]
        public async Task UploadFile_FileWithWrongDate_ThrowException()
        {
            // Arrange
            var context = new AppDbContext();
            var service = new CallService(context);
            using var stream = _fileSystem.File.OpenRead(@"c:\example_incorrect_date.csv");

            // Act
            var ex = Assert.ThrowsAsync<CsvHelper.ReaderException>(async () => await service.AddDetailsFromFile(stream));

            // Assert
            Assert.AreEqual("An unexpected error occurred.", ex.Message.Substring(0,29));
        }
    }
}