﻿using AutoMapper;
using Discount.Grpc.entities;
using Discount.Grpc.Repositories.Interfaces;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService:DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository repository, 
        ILogger<DiscountService> logger, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        
        try
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
               
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount" +
                                                                       $"with Product name = {request.ProductName} is not found"));
            }

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new KeyNotFoundException(e.Message);
            throw;
        }
       
          
            
    }   

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _repository.CreateDiscount(coupon);
        _logger.LogInformation("Discount is successfully created. Product name : {ProductName}", coupon.ProductName);
        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _repository.UpdateDiscount(coupon);
        _logger.LogInformation("Discount is successfully updated. Product name : {ProductName}", coupon.ProductName);
        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscount(request.ProductName);
        var response = new DeleteDiscountResponse()
        {
            Success = deleted
        };

        return response;
    }
}
