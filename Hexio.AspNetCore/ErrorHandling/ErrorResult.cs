namespace Hexio.AspNetCore.ErrorHandling
{
    public class ErrorResult
    {
        public int StatusCode { get; set; }

        public object Content { get; set; }
    }

    internal class ErrorNotHandledResult : ErrorResult
    {
    }
}
