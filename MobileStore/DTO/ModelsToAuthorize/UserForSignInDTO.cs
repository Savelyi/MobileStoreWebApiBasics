using System.ComponentModel.DataAnnotations;

namespace MobileStore.DTO.ModelsToAuthorize
{
    public class UserForSignInDTO : AuthorizeModel
    {
        public string ReturnUrl { get; set; }
    }
}
