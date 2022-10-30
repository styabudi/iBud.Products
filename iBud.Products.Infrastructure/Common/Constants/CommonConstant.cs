namespace iBud.Products.Infrastructure.Common.Constants;
public static class CommonConstant
{
    public struct StatusId
    {
        public const bool Active = false;
        public const bool Inactive = true;
    }
    public struct AuthMessage
    {
        public const string UserExist = "Username or Email already exist !";
        public const string UsernameExist = "Username already exist !";
        public const string EmailExist = "Email already exist !";
        public const string UserNotExist = "User not exist !";
        public const string PasswordIncorrect = "Invalid Credentials !";
        public const string EmailNotConfirmed = "Email Need to be confirmed";
        public const string UserConfirmed = "User Confirmed";
        public const string InvalidParameter = "Invalid Parameter";
    }
    public struct AuthenticationMail
    {
        public const string MailSubject = "iBud Super App Account Verificarion";
    }
}