namespace TestApi.DTO.User
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; } = 1;
       
    }
}
