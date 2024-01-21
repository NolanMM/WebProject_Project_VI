namespace WebProject_Project_VI.Services
{
    public interface ITableServices
    {
        ITableServices? SetUp(string? session_id, IConfiguration configuration);
        bool Delete_All_Data_Async();
    }
}
