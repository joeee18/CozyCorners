using CozyCorners.Core.Models.Order;

namespace CozyCorners.ViewModels
{
    public class CheckoutVM
    {
        public string Id { get; set; }
       
        public DeliveryMethod DeliveryMethod { get; set; }

       
        public Address Address { get; set; }
    }
}
