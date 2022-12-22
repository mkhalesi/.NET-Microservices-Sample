using DiscountService.Model.Services;
using DiscountService.Proto;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace DiscountService.GRPC
{
    public class GRPCDiscountService : DiscountServiceProto.DiscountServiceProtoBase
    {
        private readonly IDiscountService discountService;

        public GRPCDiscountService(IDiscountService discountService)
        {
            this.discountService = discountService;
        }
        public override Task<ResultStatusCode> GetDiscountByCode(RequestGetDiscountByCode request, ServerCallContext context)
        {
            var data = discountService.GetDiscountByCode(request.Code);
            return Task.FromResult(new ResultStatusCode()
            {
                IsSuccess = true,
                Message = "is success",
                Data = new DiscountInfo()
                {
                    Amount = data.Amount,
                    Code = data.Code,
                    Id = data.Id.ToString(),
                    Used = data.Used,
                }
            });
        }

        public override Task<ResultStatusCode> GetDiscountById(RequestGetDiscountById request, ServerCallContext context)
        {
            var data = discountService.GetDiscountById(request.Id);
            return Task.FromResult(new ResultStatusCode()
            {
                IsSuccess = true,
                Message = "is success",
                Data = new DiscountInfo()
                {
                    Amount = data.Amount,
                    Code = data.Code,
                    Id = data.Id.ToString(),
                    Used = data.Used,
                }
            });
        }

        public override Task<ResultUseDiscount> UseDiscount(RequestUseDiscount request, ServerCallContext context)
        {
            var result = discountService.UseDiscount(Guid.Parse(request.Id));
            return Task.FromResult(new ResultUseDiscount
            {
                IsSuccess = result,
            });

        }

        public override Task<ResultAddNewDiscount> AddNewDiscount(RequestAddNewDiscount request, ServerCallContext context)
        {
            var result = discountService.AddNewDiscount(request.Code, request.Amount);
            return Task.FromResult(new ResultAddNewDiscount
            {
                IsSuccess = result,
            });
        }
    }
}
