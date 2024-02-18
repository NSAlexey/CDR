using CDR.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
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
                var count = 0;
                var records = csv.GetRecordsAsync<CallDetail>();
                await foreach (var record in records)
                {
                    if (!await _dbContext.CallDetails.AsNoTracking().AnyAsync(r => r.CallerId == record.CallerId && r.Recipient == record.Recipient
                                                            && r.CallDate == record.CallDate && r.EndTime == record.EndTime))
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

        public async Task<Dictionary<long, int>> CallStatisticsByCaller(DateOnly startDate, DateOnly endDate)
        {
            var results = await _dbContext.CallDetails.Where(r => r.CallDate >= startDate && r.CallDate <= endDate).ToListAsync();
            return results.GroupBy(r => r.CallerId??0).OrderBy(r=>r.Key).ToDictionary(r => r.Key, v => v.Count());
        }

        public async Task<List<CallDetail>> CallListByCallerAndPeriod(long callerId, DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.CallDetails.Where(r => r.CallerId == callerId && r.CallDate>= startDate && r.CallDate<=endDate).ToListAsync();
        }

        public async Task<decimal> CostByCallerOnPeriod(long callerId, DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.CallDetails.Where(r => r.CallerId == callerId && r.CallDate >= startDate && r.CallDate <= endDate).SumAsync(s=>s.Cost);
        }

        public async Task<double> AverageDurationByCallerOnPeriod(long callerId, DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.CallDetails.Where(r => r.CallerId == callerId && r.CallDate >= startDate && r.CallDate <= endDate).AverageAsync(s => s.Duration);
        }

        public async Task<CallDetail> LongestCallByPeriod(DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.CallDetails.Where(r => r.CallDate >= startDate && r.CallDate <= endDate).OrderByDescending(o => o.Duration).FirstOrDefaultAsync();
        }
    }
}
