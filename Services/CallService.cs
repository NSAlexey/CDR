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
                var records = csv.GetRecords<CallDetail>();
                var count = 0;
                foreach (var record in records)
                {
                    if (_dbContext.CallDetails.FirstOrDefault(r => r.CallerId == record.CallerId && r.Recipient == record.Recipient 
                                                        && r.CallDate == record.CallDate && r.EndTime == record.EndTime) != null)
                    {
                        _dbContext.CallDetails.Add(record);
                        count++;
                    }
                }
                await _dbContext.SaveChangesAsync();
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
