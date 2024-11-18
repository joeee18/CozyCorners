using System.ComponentModel.DataAnnotations;

namespace CozyCorners.ViewModels
{
	public class RoleViewModel
	{

        public string Id { get; set; }
      
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
