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

        public Response()
        {
        }

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public void Setter(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Actualiza las propiedades de la respuesta standard
        /// </summary>
        public Response<T> Update(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;

            return this;
        }

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
