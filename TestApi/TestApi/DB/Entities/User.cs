namespace TestApi.DB.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserRoles> UserRoles { get; set; }
    }
}
