using System.ComponentModel.DataAnnotations;

namespace MobileStore.DTO.AccountModels
{
    public class UserForSignUpDTO : AuthorizeModel
    {
        [Compare("Password",ErrorMessage ="Passwords dont match")]
        public string ConfirmPassword { get; set; }

        public int RoleId { get; } = 2;
    }
}
