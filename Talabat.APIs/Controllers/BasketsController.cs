using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
   
    public class BasketsController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketsController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        //Get Or ReCreate Basket
        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
        {
            var Basket= await _basketRepository.GetBasketAsync(basketId);
            return Basket is null ? new CustomerBasket(basketId) : Ok(Basket);
            //if (Basket is null) return new CustomerBasket(basketId);
            //return Ok(Basket);
        }

        //Update Or Create
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var UpdatedOrCreatedBasket= await _basketRepository.UpdateBasketAsync(basket);
            if (UpdatedOrCreatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedOrCreatedBasket);
        }

        //Delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
