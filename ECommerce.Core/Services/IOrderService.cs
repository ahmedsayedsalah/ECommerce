using ECommerce.Core.Dtos;
using ECommerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services
{
    public interface IOrderService
    {
        Task<OrderToReturnDto?> CreateOrderAsync(string buyerEmail,string basketId,AddressDto address,int deliveryMethodId);
        Task<IReadOnlyList<OrderToReturnDto>> GetOrdersForSpecificUserAsync(string buyerEmail);
        Task<OrderToReturnDto?> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
