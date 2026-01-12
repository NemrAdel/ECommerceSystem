using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.BasketModule;
using ECommerce.Doamin.Entities.OrderModule;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Service.Specifications.OrrderSpecifications;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.OderDTOs;

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
            //1- Order Address
            var orderAddress = _mapper.Map<OrderAddressDTO, OrderAddress>(orderDTO.ShipToAddress);
            //2- Basket
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (basket is null)
                return Error.NotFound
                    ("Basket.NotFound",
                    $"The basket with id: {orderDTO.BasketId} is not found");
            
            if(basket.PaymentIntentId is null)
                return Error.Validation
                    ("Basket.PaymentIntentId.Null",
                    $"The basket with id: {orderDTO.BasketId} has no payment intent id");

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach(var items in basket.Items)
            {
                //3- Product
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(items.Id);
                if (product is null)
                    return Error.NotFound
                        ("Product.NotFound"
                        , $"the product with id :{items.Id} is not found");

                 orderItems.Add(CreateOrderItem(items, product));
                               
            }
            //4- Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderDTO.DeliveryMethodId);
            if(deliveryMethod is null)
                return Error.NotFound
                        ("DeliveryMethod.NotFound"
                        , $"the Delivery with id :{orderDTO.DeliveryMethodId} is not found");
            //5- SubTotal
            var SubTotal = orderItems.Sum(x => x.Price * x.Quantity);

            var OrderPaymentSpec=
                new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var OrderExistWithPaymentIntent=await _unitOfWork.GetRepository<Order,Guid>()
                .GetByIdAsync(OrderPaymentSpec);

            if(OrderExistWithPaymentIntent is not null)
                _unitOfWork.GetRepository<Order, Guid>().Delete(OrderExistWithPaymentIntent);
            

            //6- Create Order
            var order =new Order()
            {
                UserEmail=Email,
                Address=orderAddress,
                DeliveryMethod=deliveryMethod,
                PaymentIntentId=basket.PaymentIntentId,
                SubTotal =SubTotal,
                Items=orderItems,
            };
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            bool result = await _unitOfWork.saveChangesAsync() > 0;
            if (!result)
                 Error.Failure("Order.Failure", "there was a problem when save changes");
            return _mapper.Map<OrderToReturnDTO>(order);

        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!deliveryMethods.Any())
                return Error.NotFound("DeliveryMethod.NotFound", "No Delivery method");
            var data= _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(data);
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var OrderSpec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(OrderSpec);
            if (!orders.Any())
                return Error.NotFound("Orders.NotFound",$"No Orders Found With this email {email}");
            var data=_mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDTO>>(orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(data);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id,string email)
        {
            var orderSpec=new OrderSpecification(id,email);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(orderSpec);
            if (order is null)
                return Error.NotFound("Order.NotFound",$"Order Not Found with id: {order!.Id}");
            var data = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Result<OrderToReturnDTO>.Ok(data);   
        }

        private OrderItem CreateOrderItem(BasketItem items, Product product)
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
                Name=product.Name
            };
        }
    }
}
