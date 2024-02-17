namespace CDR.Services
{
    public interface ICallService
    {
        public Task<int> AddDetailsFromFile(Stream file);
    }
}
