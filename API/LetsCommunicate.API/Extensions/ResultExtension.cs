﻿using LetsCommunicate.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Extensions
{
    public static class ResultExtension
    {
        public static async Task<IActionResult> Process<T>(this Task<Result<T>> resultTask)
        {
            var result = await resultTask;
            switch (result.CodeResult)
            {
                case ResultCode.Ok:
                    return new OkObjectResult(result);
                case ResultCode.BadRequest:
                    return new BadRequestObjectResult(result) { StatusCode = result.Code };
                case ResultCode.NotFound:
                    return new NotFoundObjectResult(result) { StatusCode = result.Code };
                case ResultCode.Forbidden:
                    return new ObjectResult(result) { StatusCode = result.Code };
                case ResultCode.NoContent:
                    return new NoContentResult();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static async Task<IActionResult> Process(this Task<Result> resultTask)
        {
            var result = await resultTask;
            switch (result.CodeResult)
            {
                case ResultCode.Ok:
                    return new OkObjectResult(result);
                case ResultCode.BadRequest:
                    return new BadRequestObjectResult(result) { StatusCode = result.Code };
                case ResultCode.NotFound:
                    return new NotFoundObjectResult(result) { StatusCode = result.Code };
                case ResultCode.Forbidden:
                    return new ObjectResult(result) { StatusCode = result.Code };
                case ResultCode.NoContent:
                    return new NoContentResult();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
