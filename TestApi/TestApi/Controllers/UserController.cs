using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestApi.DB.Entities;
using TestApi.Validators;
using TestApi.DTO.User;
using TestApi.Domain.Services.UserServices;
using TestApi.Results;
using System.Net;
using FluentValidation;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;

namespace TestApi.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : Controller
    {
        IUserService _userService;
        CreateUserValidator _createValidator;
        UserListValidator _usersListValidator;
        AddRoleToUserValidator _addRoleToUserValidator;
        UpdateUserValidator _updateUserValidator;

        public UserController(IUserService userService, 
            CreateUserValidator createValidator, 
            UserListValidator usersListValidator,
            AddRoleToUserValidator addRoleToUserValidator,
            UpdateUserValidator updateUserValidator
            )
        {
            _userService = userService;
            _createValidator = createValidator;
            _usersListValidator = usersListValidator;
            _addRoleToUserValidator = addRoleToUserValidator;
            _updateUserValidator = updateUserValidator;
        }
        /// <summary>
        /// Return list of users with filters and sort
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-users")]
        public async Task<IActionResult> GetUsers([FromBody] UserListRequest request)
        {
            
            var result = await _usersListValidator.ValidateAsync(request);
            if(result.IsValid) 
            {
                var users = await _userService.GetUsersAsync(request);
                return Json(ApiResult<IEnumerable<User>>.Succes(users));

            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));
        }
        /// <summary>
        /// Return user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get-user")]
        public async Task <IActionResult> GetUser(int id)
        {
            var user =await _userService.GetUserByIdAsync(id);
            return Json(ApiResult<User>.Succes(user));  
        }
        
        /// <summary>
        /// Create user with spicified roleId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var valid = await _createValidator.ValidateAsync(request);
            if(valid.IsValid)
            {
                var userId = await _userService.CreateUserAsync(request);
                return Ok(ApiResult<int>.Succes(userId));

            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));
        }
        /// <summary>
        /// Add role to user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-role")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleToUserRequest request)
        {
            var valid = await _addRoleToUserValidator.ValidateAsync(request);
            if (valid.IsValid)
            {
                var userId = await _userService.AddRoleToUserAsync(request);
                return Ok(ApiResult<int>.Succes(userId));

            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));
        }

        /// <summary>
        /// Update user info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var valid = await _updateUserValidator.ValidateAsync(request);
            if (valid.IsValid)
            {
                var userId = await _userService.UpdateUserAsync(request);
                return Ok(ApiResult<int>.Succes(userId));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));
        }
        /// <summary>
        /// Delete user from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = await _userService.DeleteUserAsync(id);
            return Ok(userId);
        }
    }
}
