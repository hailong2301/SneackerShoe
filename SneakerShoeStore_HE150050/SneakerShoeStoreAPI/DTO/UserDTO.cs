namespace SneakerShoeStoreAPI.DTO
{
    public class UserDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public bool? UserStatus { get; set; }
    }
}
