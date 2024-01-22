using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Services
{
    public interface ITableServices
    {
        public ITableServices? SetUp(string? session_id, IConfiguration configuration);
        public Task<List<IData>?> Read_All_Data_Async(string? username_Authorized, string? password_Authorized);
        public Task<bool> Delete_All_Data_Async(string? username, string? password);
        public Type Get_Type();
    }
}
