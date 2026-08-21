namespace MultiShop.WebUI.Authentication
{
    public class UserAuthenticationRequiredException:Exception
    {
        public UserAuthenticationRequiredException(string message) : base(message) { }
    }
}
