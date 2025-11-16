using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.BasketModule;
using ECommerce.Service.Exceptions;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO CreateOrUpdateBasket)
        {
            var CostumerBasket = _mapper.Map<CostumerBasket>(CreateOrUpdateBasket);
            var createdOrUpdatedBasket =await _basketRepository.CreateOrUpdateBasketAsync(CostumerBasket);
            return _mapper.Map<BasketDTO>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public Task<BasketDTO?> GetBasketAsync(string basketId)
        {
            var basket= _basketRepository.GetBasketAsync(basketId);

            if (basket is null)
            {
                throw new BasketNotFoundException(basketId);
            }
            return _mapper.Map<Task<BasketDTO?>>(basket);
        }
    }
}
