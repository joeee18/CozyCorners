using CozyCorners.Core;
using CozyCorners.Core.Models.Identity;
using CozyCorners.Core.Models.Order;
using CozyCorners.Core.Repositories.Contract;
using CozyCorners.Core.Services.Contract;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace CozyCorners.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderServices _orderServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartRepository cartRepository;

        public OrderController(IUnitOfWork unitOfWork,IOrderRepository orderRepository,IOrderServices orderServices,UserManager<AppUser> userManager,ICartRepository cartRepository)
        {
            this.unitOfWork = unitOfWork;
            this.orderRepository = orderRepository;
            _orderServices = orderServices;
           _userManager = userManager;
            this.cartRepository = cartRepository;
        }

       [Authorize]
       public async Task<ActionResult> CreateOrder(CheckoutVM checkoutVM, string PaymentMethod)
        {
            var user = await _userManager.GetUserAsync(User);
            var orderresult = await _orderServices.CreateOrderAsync(user.Email, checkoutVM.Id, checkoutVM.DeliveryMethod.Id, checkoutVM.Address);
           var cart=await cartRepository.GetCustomerCartAsync(user.Id);
             
            if (orderresult is null)
                return View("OrderFailure");
            if (PaymentMethod == "CashOnDelivery")
            {
                await cartRepository.DeleteCartAsync(cart.Id);
                return View("OrderSucess");
            }
            else
            {
                var domain = "https://localhost:44305/";

                var options = new SessionCreateOptions
                {
                    SuccessUrl=domain+$"Order/OrderConfirm",
                    CancelUrl=domain+ $"Account/Signin",
                    LineItems=new List<SessionLineItemOptions>(),
                    Mode="payment",
                   

                };
                foreach (var item in cart.CartItems)
                {
                    var SessionListItem = new SessionLineItemOptions
                    {
                        PriceData=new SessionLineItemPriceDataOptions
                        {
                            UnitAmount=(long)(item.Price*item.Quantity),
                            Currency="USD",
                            ProductData=new SessionLineItemPriceDataProductDataOptions
                            {
                                Name=item.ProductName.ToString()
                                
                            }
                        },
                        Quantity=item.Quantity
                    };
                    options.LineItems.Add(SessionListItem);
                }
                var services = new SessionService();
                Session session = services.Create(options);
                TempData["Session"] = session.Id;
                Response.Headers.Add("Location", session.Url);
                await cartRepository.DeleteCartAsync(cart.Id);
                return new StatusCodeResult(303);
                //return View("OrderSucess");
            }



            
        }

        
        public IActionResult OrderConfirm()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus=="paid")
            {
                return View("OrderSucess");
            }
            return View("OrderFailure");
        }

        [HttpGet]
        public async Task<ActionResult<Order>> GetOrdersForUser(string? id)
        {
            if (id == null) return View("NoOrders");
            var useremail = await _userManager.FindByIdAsync(id);

           var orders = await orderRepository.GetOrdersForUser(useremail.Email);
            if (orders.Count()==0) return View("NoOrders");
            return View(orders);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> AllOrders()
        {
            var orders = await orderRepository.GetAllAsync();
           
            return View(orders);
        }
        [HttpGet]
        public async Task<ActionResult<Order>> Details(int id)
        {
            var order = await orderRepository.GetOrderById(id);

            return View(order);
        }
       
        public async Task<ActionResult<Order>> Confirm(int id)
        {
            var order = await orderRepository.GetOrderById(id);
            order.Status = OrderStatus.Processing;
            orderRepository.update(order);
            await unitOfWork.Complet();
            return RedirectToAction(nameof(AllOrders));
        }
        
        public async Task<ActionResult<Order>> Cancel(int id)
        {
            var order = await orderRepository.GetOrderById(id);
            order.Status = OrderStatus.Cancelled;
            orderRepository.update(order);
            await unitOfWork.Complet();
            return RedirectToAction(nameof(AllOrders));
        }

    }
}
