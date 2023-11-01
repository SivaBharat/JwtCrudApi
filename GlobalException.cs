namespace JwtCrud
{
    public class GlobalException : Exception
    {
        public GlobalException(string message) : base(message)
        {
        }
    }
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
