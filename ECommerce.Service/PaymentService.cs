using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.OrderModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = ECommerce.Doamin.Entities.ProductModule.Product;

namespace ECommerce.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            var Skey = _configuration["Stripe:SKey"];
            if(Skey is null)
                return Error.Failure("Stripe secret key is not configured");
            StripeConfiguration.ApiKey = Skey;

            var Basket=await _basketRepository.GetBasketAsync(BasketId);
            if(Basket is null)
                return Error.NotFound("Basket not found");

            if(Basket.DeliveryMethodId is null)
                return Error.Validation("Delivery method is not selected");

            var DeliveryMethod=await _unitOfWork.GetRepository<DeliveryMethod,int>()
                .GetByIdAsync(Basket.DeliveryMethodId.Value);
            if(DeliveryMethod is null)
                return Error.NotFound("Delivery method not found");

            Basket.ShippingPrice = DeliveryMethod.Price;
            foreach (var item in Basket.Items)
            {
                var Product = await _unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id);
                if(Product is null)
                    return Error.NotFound($"Product with id {item.Id} not found");
                item.Price = Product.Price;
                item.ProductName = Product.Name;
                item.PictureUrl=Product.PictureUrl;
            }
            long Amount =(long)(Basket.Items.Sum(x => x.Price * x.Quantity) * 100); //must by cents


            var stripeService=new PaymentIntentService();

            if(Basket.PaymentIntentId is null)
            {
                // Integiration with Stripe to create payment intentstripe

                var options=new PaymentIntentCreateOptions
                {
                    Amount = Amount + (long)(Basket.ShippingPrice * 100),
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await stripeService.CreateAsync(options); //External Call here
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                var options=new PaymentIntentUpdateOptions
                {
                    Amount = Amount + (long)(Basket.ShippingPrice * 100),
                };
                await stripeService.UpdateAsync(Basket.PaymentIntentId,options);
            }

            //Update the basket with the new data in redis
            await _basketRepository.CreateOrUpdateBasketAsync(Basket);

            return _mapper.Map<BasketDTO>(Basket);
        }
    }
}
