namespace ChatApp.Domain.Constants;

public static class AuthServiceConstants
{
    public static string GenericRequiredMessage(string property) => $"The {property} is required!";

    public static class LoginInput
    {
        public const string RequiredErrorMessage = $"The Username is required!";
    }

    public static class RegisterInput
    {
    }
}