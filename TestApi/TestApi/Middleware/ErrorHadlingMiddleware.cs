using System.Text.Json;
using System.Net;
using System.Runtime.CompilerServices;
using TestApi.Domain.Exceptions;
using TestApi.Results;

namespace TestApi.Middleware
{
    public class ErrorHadlingMiddleware
    {
        RequestDelegate next;

        public ErrorHadlingMiddleware(RequestDelegate next)
        {
            this.next = next;   
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context,ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = ex switch
            {
                NotFoundException _ => HttpStatusCode.NotFound,
                BadRequestException _ => HttpStatusCode.BadRequest,
                ConflictException _ => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            var errors = new List<string>() { ex.Message};

            var result = JsonSerializer.Serialize(ApiResult<string>.Failure(code, errors));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);

        }
    }
}
