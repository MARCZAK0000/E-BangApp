namespace E_BangDomain.ResponseDtos.SharedResponseDtos
{
    public class SuccessResponseDto
    {

		public bool IsSuccess
		{
			get { return _isSuccess; }
			set { 
				_isSuccess = value;
				UpdateMessage();
				
			}
		}
		private bool _isSuccess;

		public string Message
        {
			get { return _message; }
			set {
                _message = value;
            }
		}
		private string _message;

		protected virtual void UpdateMessage()
		{
            _message = IsSuccess ? "Operation completed successfully." : "Operation failed.";
        }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
