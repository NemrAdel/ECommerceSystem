
namespace ECommerce.Shared.DTOs.SecurityDTOs
{
    public class ResetPasswordParams
    {
        public int OTP { get; set; }
        public string Password { get; set; } = null!;
    }
}
