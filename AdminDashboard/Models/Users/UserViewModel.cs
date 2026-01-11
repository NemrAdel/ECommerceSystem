namespace AdminDashboard.Models.Users
{
    public class UserViewModel
    {
        public string Id { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public IEnumerable<string> Roles { get; set; } = default!;
    }
}
