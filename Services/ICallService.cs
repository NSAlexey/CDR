using CDR.Entities;

namespace CDR.Services
{
    public interface ICallService
    {
        public Task<int> AddDetailsFromFile(Stream file);
        public Task<Dictionary<long, int>> CallStatisticsByCaller(DateOnly startDate, DateOnly endDate);
        public Task<List<CallDetail>> CallListByCallerAndPeriod(long callerId, DateOnly startDate, DateOnly endDate);
        public Task<decimal> CostByCallerOnPeriod(long callerId, DateOnly startDate, DateOnly endDate);
        public Task<double> AverageDurationByCallerOnPeriod(long callerId, DateOnly startDate, DateOnly endDate);
        public Task<CallDetail> LongestCallByPeriod(DateOnly startDate, DateOnly endDate);
    }
}
