using TestApi.DB.Entities;
using TestApi.DTO.User;

namespace TestApi.Domain.Services.UserServices
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync(UserListRequest request);
        public Task<User> GetUserByIdAsync(int userId);
        public Task<int> AddRoleToUserAsync(AddRoleToUserRequest request);
        public Task<int> CreateUserAsync(CreateUserRequest request);
        public Task<int> DeleteUserAsync(int userId);
        public Task<int> UpdateUserAsync(UpdateUserRequest request);
    }
}
