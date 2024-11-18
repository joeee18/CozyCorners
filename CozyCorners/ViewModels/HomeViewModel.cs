using CozyCorners.Core.Models;

namespace CozyCorners.ViewModels
{
	public class HomeViewModel
	{
		public IReadOnlyList<Category> Categories { get; set; }
		public IReadOnlyList<Product> Products { get; set; }
	}
}
