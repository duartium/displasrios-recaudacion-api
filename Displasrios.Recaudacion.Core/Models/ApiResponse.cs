using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class Response<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

    }
    public class SuccessResponse<T> : Response<T>
    {
    }

    public class ErrorResponse<T> : Response<T>
    {
        [JsonPropertyName("error_code")]
        public string ErrorCode { get; set; }
    }
}
