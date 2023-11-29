using ApiGateway.ForWeb.Models.Dtos;
using DiscountService.Proto;

namespace ApiGateway.ForWeb.Models.DiscountServices;

public class DiscountService : IDiscountService
{
    private readonly DiscountServiceProto.DiscountServiceProtoClient _discountClient;
    //private readonly IConfiguration configuration;
    //private readonly GrpcChannel channel;
    public DiscountService(DiscountServiceProto.DiscountServiceProtoClient discountClient)
    {
        _discountClient = discountClient;
        //this.configuration = configuration;
        //channel = GrpcChannel.ForAddress(configuration["MicroserviceAddress:DiscountGrpc:Uri"]);
    }

    public ResultDto<DiscountDto> GetDiscountByCode(string Code)
    {
        //var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
        var result = _discountClient.GetDiscountByCode(new RequestGetDiscountByCode
        {
            Code = Code
        });

        if (result.IsSuccess)
        {
            return new ResultDto<DiscountDto>
            {
                Data = new DiscountDto
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
        return new ResultDto<DiscountDto>
        {
            IsSuccess = false,
            Message = result.Message,
        };
    }

    public ResultDto<DiscountDto> GetDiscountById(Guid Id)
    {
        //var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
        var result = _discountClient.GetDiscountById(new RequestGetDiscountById
        {
            Id = Id.ToString(),
        });

        if (result.IsSuccess)
        {
            return new ResultDto<DiscountDto>
            {
                Data = new DiscountDto
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
        return new ResultDto<DiscountDto>
        {
            IsSuccess = false,
            Message = result.Message,
        };
    }

    public ResultDto UseDiscount(Guid DiscountId)
    {
        //var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
        var result = _discountClient.UseDiscount(new RequestUseDiscount
        {
            Id = DiscountId.ToString(),
        });
        return new ResultDto
        {
            IsSuccess = result.IsSuccess
        };
    }
}