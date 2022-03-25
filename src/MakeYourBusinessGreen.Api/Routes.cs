namespace MakeYourBusinessGreen.Api;
public class Routes
{
    public const string Base = "api";

    public static class Auth
    {
        public const string SignIn = Base + "/auth/signin";
        public const string SignUp = Base + "/auth/signup";
        public const string ForgotPassword = Base + "/auth/forgotpassword";
        public const string ResetPassword = "resetpassword";
        public const string ChangePassword = Base + "/auth/changepassword";
        public const string RefreshToken = Base + "/auth/refreshtoken";
        public const string AddUserToRole = Base + "/auth/addusertorole";
    }

    public static class Office
    {
        public const string Create = Base + "/office";
        public const string Update = Base + "/office";
        public const string Delete = Base + "/office/{id:guid}";
        public const string Get = Base + "/office/{id:guid}/";
        public const string GetAll = Base + "/office";
        public const string SearchByLocation = Base + "/office/bylocation";
        public const string SearchByName = Base + "/office/byname";
    }

    public static class Suggestion
    {
        public const string Create = Base + "/suggestion";
        public const string UpdateStatus = Base + "/suggestion";
        public const string Delete = Base + "/suggestion/{id:guid}";
        public const string Get = Base + "/suggestion/{id:guid}/";
        public const string GetAll = Base + "/suggestion";
        public const string GetForUser = Base + "/suggestion/{userId:string}";
        public const string GetForCurrentUser = Base + "/mysuggestions";
        public const string SearchByTitle = Base + "/suggestion/search/bytitle/";
        public const string GetByStatus = Base + "/suggestions/search/bystatus/";
    }

    public static class User
    {
        public const string Get = Base + "/user/{userId}";
        public const string GetAll = Base + "/user";
        public const string Update = Base + "/user";
    }

}
