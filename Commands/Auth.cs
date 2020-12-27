using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VkAudioProject.ViewModels;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;

namespace VkAudioProject.Commands
{
    class Auth
    {
        public static readonly VkApi Api = new VkApi(new ServiceCollection().AddAudioBypass());

        public static User CurrentUser = new User();

        public static async void Login(string token)
        {
            await Api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = token
            });
            CurrentUser = GetCurrentUser();
        }

        public static async Task Login(string login, string password)
        {
            await Api.AuthorizeAsync(new ApiAuthParams
            {
                Login = login,
                Password = password
            });
            if (Api.IsAuthorized)
            {
                Settings.Instance.Token = Api.Token;
                Settings.Instance.Save();
                CurrentUser = GetCurrentUser();
            }
        }

        public static async Task Login(string login, string password, string twoAuth)
        {
            await Api.AuthorizeAsync(new ApiAuthParams
            {
                Login = login,
                Password = password,
                ForceSms = true,
                TwoFactorAuthorization = () => twoAuth
            });
            if (Api.IsAuthorized)
            {
                Settings.Instance.Token = Api.Token;
                Settings.Instance.Save();
                CurrentUser = GetCurrentUser();
            }
        }

        private static User GetCurrentUser()
        {
            return Api.Users.Get(new List<long>(), VkNet.Enums.Filters.ProfileFields.ScreenName).FirstOrDefault();
        }
    }
}
