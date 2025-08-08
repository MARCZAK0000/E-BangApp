namespace E_BangDomain.ResultsPattern
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public static Result Success()
        {
            return new Result { IsSuccess = true };
        }
        public static Result Failure(IEnumerable<Errors> errorMessage)
        {
            return new Result { IsSuccess = false, ErrorMessage = string.Join(", ", errorMessage) };
        }
    }
}
