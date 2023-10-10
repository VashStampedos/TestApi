using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;
using TestApi.DB.Context;
using TestApi.DB.Entities;
using TestApi.Domain.Exceptions;
using TestApi.Domain.Services.Application;
using TestApi.DTO.User;
using TestApi.Enums;
using TestApi.Results;

namespace TestApi.Domain.Services.UserServices
{
    public class UserService : ApplicationContextService, IUserService
    {
        
        public UserService(ApplicationContext context):base(context) 
        {
            
        }

        private User InitializeUser(string name, int age, string email)
        {
            User user = new User();
            user.Name = name;
            user.Age = age;
            user.Email = email;

            return user;
        }

        private async Task<IEnumerable<User>> FilterUsers(string filterBy, string filterValue, SortEnum sortBy, string sortValue)
        {
            IEnumerable<User> users;
            switch (filterBy.ToLower())
            {
                case "name":
                    users = await context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).Where(x => x.Name == filterValue).ToListAsync();
                    break;
                case "age":
                    users = await context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).Where(x => x.Age == Convert.ToInt32(filterValue)).ToListAsync();
                    break;
                case "email":
                    users = await context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).Where(x => x.Email == filterValue).ToListAsync();
                    break;
                case "role":
                    //var userroles = context.UserRoles.AsNoTracking().Where(x => x.RoleId == Convert.ToInt32(filterValue));
                    //users = await userroles.Select(x => x.User).ToListAsync();
                    var userroles = context.UserRoles.AsNoTracking().Where(x => x.RoleId == Convert.ToInt32(filterValue)).Select(x=> x.User).ToList();
                    var users2 = context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).ToList();
                    var compare = users2.Intersect(userroles, new UserComparer()).ToList();
                    //users = await userroles.Select(x => x.User).ToListAsync();
                    users = compare;


                    break;


                default:
                    users = context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role);
                    break;
            }
            if (sortValue != string.Empty)
            {
                return SortUsers(sortBy, sortValue, users);
            }
            else
            {
                return users;
            }

        }
        private IEnumerable<User> CreateUserList(IEnumerable<UserRoles> filteredUserRoles)
        {
            var userList = new List<User>();
            foreach (var userRole in filteredUserRoles)
            {
                if (!userList.Any(x => x.Id == userRole.UserId))
                {
                    userList.Append(userRole.User);
                }
            }
            return userList;
        }

        private IEnumerable<User> SortUsers(SortEnum sortBy, string sortValue, IEnumerable<User> users)
        {

            if (sortBy == SortEnum.SortByAsc)
            {
                switch (sortValue.ToLower())
                {
                    case "name":
                        return users.OrderBy(x => x.Name);
                    case "age":
                        return users.OrderBy(x => x.Age);
                    case "email":
                        return users.OrderBy(x => x.Email);
                    case "role":
                        var asdasd = users.Select(x =>
                        {
                            x.UserRoles = x.UserRoles.OrderBy(x => x.RoleId).AsEnumerable();
                            return x;
                        });
                        return asdasd;

                    default:
                        return users;
                }

            }
            else if (sortBy == SortEnum.SortByDesc)
            {
                switch (sortValue.ToLower())
                {
                    case "name":
                        return users.OrderByDescending(x => x.Name);
                    case "age":
                        return users.OrderByDescending(x => x.Age);
                    case "email":
                        return users.OrderByDescending(x => x.Email);
                    case "role":
                        return users.Select(x =>
                        {
                            x.UserRoles = x.UserRoles.OrderByDescending(x => x.RoleId).AsEnumerable();
                            return x;
                        });
                    default:
                        return users;
                }
            }
            else return users;
        }

       

        public async Task<IEnumerable<User>> GetUsersAsync(UserListRequest request)
        {
           
            var filteredUsers = await FilterUsers(request.FilterBy, request.FilterValue, request.SortBy, request.SortValue);
            var users = filteredUsers.Skip(request.Offset).Take(request.Count);
            return users;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.Id == userId);
            _ = user ?? throw new NotFoundException($"User with id {userId} not found");
            return user;
        }

        public async Task<int> AddRoleToUserAsync(AddRoleToUserRequest request)
        {
            var user = await GetUserByIdAsync(request.UserId);
            await CheckUserRoleAsync(user.Id, request.RoleId);
            await context.UserRoles.AddAsync(new UserRoles() { UserId = request.UserId, RoleId = request.RoleId });
            await context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int> CreateUserAsync(CreateUserRequest request)
        {
            await CheckUserEmailAsync(request.Email);
            await ChecRoleAsync(request.RoleId);

            var user = InitializeUser(request.Name, request.Age, request.Email);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var result = await AddRoleToUserAsync(new AddRoleToUserRequest { UserId = user.Id, RoleId = request.RoleId });
            return result;

        }
        public async Task<int> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user.Id;
        }
        public async Task<int> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await GetUserByIdAsync(request.Id);
            await CheckUserEmailForUpdateAsync(user.Id, request.Email);

            user.Name = request.Name;
            user.Age = request.Age;
            user.Email = request.Email;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user.Id;
        }

        private async Task ChecRoleAsync(int roleId)
        {
            var contains = await context.Roles.AsNoTracking().AnyAsync(x => x.Id == roleId);
            if (!contains)
                throw new NotFoundException($"Role not found");

        }

        private async Task CheckUserRoleAsync(int userId, int roleId)
        {
            var contains = await context.UserRoles.AsNoTracking().AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
            if (contains)
                throw new ConflictException($"User with role exists");

        }

        private async Task CheckUserEmailAsync(string email)
        {
            var contains = await context.Users.AsNoTracking().AnyAsync(x => x.Email.ToLower() == email.ToLower());
            if (contains)
                throw new ConflictException($"User with email exists");
        }
        private async Task CheckUserEmailForUpdateAsync(int userId, string email)
        {
            var contains = await context.Users.AsNoTracking().AnyAsync(x => x.Email.ToLower() == email.ToLower() && x.Id != userId);
            if (contains)
                throw new ConflictException($"User with email exists");
        }

    }

    public class UserComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
