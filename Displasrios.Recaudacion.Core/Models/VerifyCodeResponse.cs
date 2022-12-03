namespace Displasrios.Recaudacion.Core.Models
{
    public class VerifyCodeResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ChangePasswordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
