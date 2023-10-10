﻿namespace TestApi.DB.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserRoles> UserRoles { get; set; }
    }
}
