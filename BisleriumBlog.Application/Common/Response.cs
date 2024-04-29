using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BisleriumBlog.Application.Common
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Result { get; set; }
    }
}
