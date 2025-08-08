namespace E_BangDomain.ResultsPattern
{
    public class Errors(string errorName, string errorMessage)
    {
        public string ErrorName { get; set; } = errorName;
        public string ErrorMessage { get; set; } = errorMessage;

        public override string ToString()
        {
            return $"{ErrorName}: {ErrorMessage}";
        }
    }
}
