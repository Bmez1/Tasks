using SharedKernel;
using Web.Api.Infrastructure;
using static Web.Api.Infrastructure.CustomResults;

namespace Web.Api.Extensions;

public static class ResultExtensions
{
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}

public static class ApiResponseExtension
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        return result.IsSuccess ? Results.Ok(ApiResponseSuccessful<T>.Create(result.Value, result.TotalData)) :
            CustomResults.Problem(result);
    }
}
