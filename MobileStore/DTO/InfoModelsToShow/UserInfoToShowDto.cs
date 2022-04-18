using System.Collections.Generic;

namespace MobileStore.DTO.InfoModelsToShow
{
    public class UserInfoToShowDto
    {

        public UserInfoToShowDto()
        {
            UserRoles = new List<string>();
        }
        public string Name { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
