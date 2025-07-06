namespace E_BangDomain.MaybePattern
{
    public class Maybe<T> where T : class
    {
        public Maybe(T? value)
        {
            Value = value;
            HasValue = value is not null;
        }

        public T? Value { get; set; }
        public bool HasValue {  get; set; } 

        
    }
}
