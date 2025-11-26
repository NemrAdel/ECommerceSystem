using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.OrderModule;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.OderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IMapper mapper,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            var orderAddress = _mapper.Map<OrderAddressDTO, OrderAddress>(orderDTO.Address);
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (basket is null)
                return Error.NotFound
                    ("Basket.NotFound",
                    $"The basket with id: {orderDTO.BasketId} is not found");
            
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach(var items in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(items.Id);
                if (product is null)
                    return Error.NotFound
                        ("Product.NotFound"
                        , $"the product with id :{items.Id} is not found");

                orderItems.Add(CreateOrderItem(items, product));
                               
            }

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderDTO.DeliveryMethodId);
            if(deliveryMethod is null)
                return Error.NotFound
                        ("DeliveryMethod.NotFound"
                        , $"the Delivery with id :{orderDTO.DeliveryMethodId} is not found");

            var SubTotal = orderItems.Sum(x => x.Price * x.Quantity);
            var order=new Order()
            {
                UserEmail=Email,
                Address=orderAddress,
                DeliveryMethod=deliveryMethod,
                SubTotal=SubTotal,
                Items=orderItems,
            };
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            bool result = await _unitOfWork.saveChangesAsync() > 0;
            if (!result)
                 Error.Failure("Order.Failure", "there was a problem when save changes");
            return _mapper.Map<OrderToReturnDTO>(order);
        }

        private async Task<OrderItem> CreateOrderItem(Doamin.Entities.BasketModule.BasketItem items, Product product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered()
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl,
                },
                Price = product.Price,
                Quantity = items.Quantity,
            };
        }
    }
}
