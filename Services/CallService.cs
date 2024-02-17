using CDR.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CDR.Services
{
    public class CallService : ICallService
    {
        private readonly AppDbContext _dbContext;

        public CallService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddDetailsFromFile(Stream file)
        {
            try
            {
                var reader = new StreamReader(file);
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", HasHeaderRecord = true };
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<CallDetail>().ToArray();
                _dbContext.CallDetails.AddRange(records);
                await _dbContext.SaveChangesAsync();
                return records.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
