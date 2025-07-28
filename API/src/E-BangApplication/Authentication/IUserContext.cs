namespace E_BangApplication.Authentication
{
    public interface IUserContext
    {
        CurrentUser GetCurrentUser();

        string GetRefreshToken();
    }
}
