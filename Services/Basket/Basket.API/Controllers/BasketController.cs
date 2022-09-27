using System.Net;
using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository basketRepository)
    {
        _repository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }

    
    [HttpGet("{userName}",Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart),(int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _repository.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart),(int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart shoppingCart)
    {
        return Ok(await _repository.UpdateBasket(shoppingCart));
    }
    
    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(ShoppingCart),(int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteBasket(string shoppingCartId)
    {
        await _repository.DeleteBasket(shoppingCartId);
        return Ok();
    }
}