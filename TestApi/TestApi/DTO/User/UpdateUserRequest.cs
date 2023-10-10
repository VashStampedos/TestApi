namespace TestApi.DTO.User
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string  Email { get; set; }
    }
}
