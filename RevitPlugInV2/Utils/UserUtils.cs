using System.Threading.Tasks;
using System;
using RevitPlugInV2.Dtos;

namespace RevitPlugInV2.Utils
{
    public static class UserUtils
    {
        public static UserLoggedDto LoggedInUser { get; set; }

        public static async Task<bool> ManuallyLogin(string username, string password)
        {
            try
            {
                var token = await APIUtils.GetAuthTokenAsync(username, password);
                if (!string.IsNullOrEmpty(token))
                {
                    var userInfo = await APIUtils.GetUserInfoAsync(token);
                    if (userInfo != null)
                    {
                        SetLoggedInUser(userInfo);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return false;
            }
        }

        public static void SetLoggedInUser(UserLoggedDto user)
        {
            LoggedInUser = user;
        }

        public static void Logout()
        {
            LoggedInUser = null;
        }
        public static bool IsUserLoggedIn()
        {
            return LoggedInUser != null;
        }


    }
}
