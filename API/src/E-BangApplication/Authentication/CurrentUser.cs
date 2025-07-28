namespace E_BangApplication.Authentication
{
    public class CurrentUser
    {

        public CurrentUser(string accountID, string emailAddress, List<string> roles)
        {
            AccountID = accountID;
            EmailAddress = emailAddress;
            Roles = roles;
        }

        public string AccountID { get; set; }

        public string EmailAddress { get; set; }

        public List<string> Roles { get; set; }


    }
}
