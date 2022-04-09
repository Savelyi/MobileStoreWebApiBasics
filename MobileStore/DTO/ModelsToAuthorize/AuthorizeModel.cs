using System.ComponentModel.DataAnnotations;

namespace MobileStore.DTO.ModelsToAuthorize
{
    public abstract class AuthorizeModel
    {
        [Required(ErrorMessage = "UserName is Required")]
        [MaxLength(15, ErrorMessage = "Can not be more than 15 letters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [StringLength(20,ErrorMessage = "Field should have at least 5 characters and no more than 20")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
