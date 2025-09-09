namespace App.EmailHelper.Shared.Enums
{
    public enum EEmailHeaderType
    {
        Default = 0,
        Custom = 1
    }
    public enum EEmailBodyType
    {
        Registration = 0,
        ConfirmEmail = 1,
        ChangePassword = 2,
        TwoWayToken = 3,
    }
    public enum EEmailFooterType
    {
        Defualt = 0,
        Custom = 1
    }
}
