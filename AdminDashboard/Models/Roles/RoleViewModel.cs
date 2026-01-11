using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(256, ErrorMessage = "Name Cannot Exceed 256 Characters")]
        public string Name { get; set; } = default!;
    }
}
