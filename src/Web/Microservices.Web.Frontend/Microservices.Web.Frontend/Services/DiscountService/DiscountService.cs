using System;
using DiscountService.Proto;
using Grpc.Net.Client;
using Microservices.Web.Frontend.Models.DTO;
using Microsoft.Extensions.Configuration;

namespace Microservices.Web.Frontend.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private GrpcChannel grpcChannel;
        private readonly IConfiguration configuration;
        public DiscountService(IConfiguration configuration)
        {
            grpcChannel = GrpcChannel.ForAddress(configuration["MicroServiceAddress:ApiGatewayForWeb:Uri"]);
        }
        public ResultDTO<DiscountDTO> GetDiscountByCode(string code)
        {
            var grpc_DiscountService = new DiscountServiceProto.DiscountServiceProtoClient(grpcChannel);
            var result = grpc_DiscountService.GetDiscountByCode(new RequestGetDiscountByCode { Code = code });

            var responseStatus = getResponseStatusCode<DiscountDTO>(result);
            if (responseStatus.IsSuccess)
                responseStatus.Data = new DiscountDTO()
                {
                    Code = result.Data.Code,
                };
            return responseStatus;
        }

        public ResultDTO<DiscountDTO> GetDiscountById(Guid Id)
        {
            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(grpcChannel);
            var result = grpc_discountService.GetDiscountById(new RequestGetDiscountById
            {
                Id = Id.ToString(),
            });

            if (result.IsSuccess)
            {
                return new ResultDTO<DiscountDTO>
                {
                    Data = new DiscountDTO
                    {
                        Amount = result.Data.Amount,
                        Code = result.Data.Code,
                        Id = Guid.Parse(result.Data.Id),
                        Used = result.Data.Used
                    },
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                };
            }
            return new ResultDTO<DiscountDTO>
            {
                IsSuccess = false,
                Message = result.Message,
            };
        }

        public ResultDTO UseDiscount(Guid DiscountId)
        {
            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(grpcChannel);
            var result = grpc_discountService.UseDiscount(new RequestUseDiscount
            {
                Id = DiscountId.ToString(),
            });
            return new ResultDTO
            {
                IsSuccess = result.IsSuccess
            };
        }


        #region helper methods

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

        #endregion

    }
}
