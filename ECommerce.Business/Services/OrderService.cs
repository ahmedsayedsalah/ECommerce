using AutoMapper;
using ECommerce.Core;
using ECommerce.Core.Dtos;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using ECommerce.Core.Repositories;
using ECommerce.Core.Services;
using ECommerce.Core.Specifications.Order_Specs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;
        private readonly IConfiguration configuration;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPaymentService paymentService,
            IConfiguration configuration
            )
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.paymentService = paymentService;
            this.configuration = configuration;
        }
        public async Task<OrderToReturnDto?> CreateOrderAsync(string buyerEmail, string basketId, AddressDto addressDto, int deliveryMethodId)
        {
            // 1. Get Basket From Baskets Repo
            var basket = await basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket(Id,Quantity) From Product Repo
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // 3. Calculate SubTotal
            var subTotal = orderItems.Sum(ot => ot.Quantity * ot.Price);

            // 4. Get DeliveryMethod From DeliveryMethod Repo  
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            basket.ShippingCost= deliveryMethod.Cost;

            // 5. Map AddressDto to Address
            var address = mapper.Map<Address>(addressDto);


            // validate more order in one basket
            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existOrder = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (existOrder is not null)
            {
                unitOfWork.Repository<Order>().Delete(existOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            // 6. Create Order
            var order = new Order(buyerEmail, address, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            // 7. Save To DB 
            await unitOfWork.Repository<Order>().Add(order);

            var affectedCount = await unitOfWork.CompleteAsync();

            return affectedCount > 0 ? mapper.Map<OrderToReturnDto>(order) : null;
        }

        public async Task<OrderToReturnDto?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var specs = new OrderSpecification(orderId, buyerEmail);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(specs);
            var orderDto= mapper.Map<OrderToReturnDto>(order);

            return orderDto;
        }

        public async Task<IReadOnlyList<OrderToReturnDto>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var specs = new OrderSpecification(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(specs);

            var ordersDto = mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

            return ordersDto;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

    }
}
