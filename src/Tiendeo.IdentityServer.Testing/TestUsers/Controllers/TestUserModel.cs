using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Testing.TestUsers.Controllers
{
    public class TestUserModel
    {
        [Required(ErrorMessage = "Missing SubjectId")]
        [StringLength(500, ErrorMessage = "Invalid length for SubjectId", MinimumLength = 1)]
        public string SubjectId { get; set; }
        [Required(ErrorMessage = "Missing SubjectId")]
        [StringLength(100, ErrorMessage = "Invalid length for Username", MinimumLength = 1)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Missing Password")]
        [StringLength(100, ErrorMessage = "Invalid length for Password", MinimumLength = 1)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Missing Name")]
        [StringLength(100, ErrorMessage = "Invalid length for Name", MinimumLength = 1)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Missing GivenName")]
        [StringLength(100, ErrorMessage = "Invalid length for GivenName", MinimumLength = 1)]
        public string GivenName { get; set; }
        [Required(ErrorMessage = "Missing FamilyName")]
        [StringLength(100, ErrorMessage = "Invalid length for FamilyName", MinimumLength = 1)]
        public string FamilyName { get; set; }
        [Required(ErrorMessage = "Missing Email")]
        [StringLength(500, ErrorMessage = "Invalid length for Email", MinimumLength = 1)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Missing Roles")]
        public List<string> Roles { get; set; }

    }
}
