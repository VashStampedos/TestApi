using System.Linq;
using System.Net;

namespace TestApi.Results
{
    public class ApiResult<T>
    {
        public bool Succeeded { get; set; }
        public HttpStatusCode Code{ get; set; }
        public IEnumerable<string> Error { get; set; }
        public T Data { get; set; }

        public ApiResult()
        {
            
        }

        public ApiResult(bool succeeded, HttpStatusCode code, IEnumerable<string> error, T data)
        {
            Succeeded = succeeded;
            Code = code;
            Error = error;
            Data = data;
        }

        public static ApiResult<T> Succes(T data) 
            => new ApiResult<T>(true, HttpStatusCode.OK, Enumerable.Empty<string>(), data);
        public static ApiResult<T> Failure(HttpStatusCode code, IEnumerable<string> error) 
            => new ApiResult<T>(false, code, error, default);
    }
}
