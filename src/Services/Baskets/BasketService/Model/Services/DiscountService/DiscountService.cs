using System;
using BasketService.Model.DTOs;
using BasketService.Model.DTOs.Discount;
using DiscountService.Proto;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace BasketService.Model.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private IConfiguration _configuration;
        private GrpcChannel channel;
        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["MicroServiceAddress:DiscountGrpc:Uri"]);
        }

        public DiscountDTO GetDiscountByCode(string code)
        {
            var grpc_DiscountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);

            var result = grpc_DiscountService.GetDiscountByCode(
                new RequestGetDiscountByCode()
                {
                    Code = code
                });

            var responseStatus = getResponseStatusCode<DiscountDTO>(result);
            if (responseStatus.IsSuccess)
                responseStatus.Data = new DiscountDTO()
                {
                    Code = result.Data.Code,
                };
            return responseStatus.Data;
        }

        public DiscountDTO GetDiscountById(Guid discountId)
        {
            var grpc_DiscountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);

            var result = grpc_DiscountService.GetDiscountById(
                new RequestGetDiscountById()
                {
                    Id = discountId.ToString()
                });

            var responseStatus = getResponseStatusCode<DiscountDTO>(result);
            if (responseStatus.IsSuccess)
                responseStatus.Data = new DiscountDTO()
                {
                    Amount = result.Data.Amount,
                    Code = result.Data.Code,
                    Id = Guid.Parse(result.Data.Id),
                    Used = result.Data.Used
                };
            return responseStatus.Data;
        }

        private static ResultDTO<T> getResponseStatusCode<T>(ResultStatusCode response)
            where T : class
        {
            if (response.IsSuccess)
                return new ResultDTO<T>()
                {
                    IsSuccess = true,
                };

            return new ResultDTO<T>()
            {
                IsSuccess = false,
                ErrorMessage = response.Message
            };
        }
    }
}