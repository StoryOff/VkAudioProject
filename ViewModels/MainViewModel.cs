using VkAudioProject.Commands;
using VkAudioProject.View;
using static VkAudioProject.ViewModels.Settings;

namespace VkAudioProject.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Instance.Load();

            if (Instance.Token == null)
            {
                new LoginView().ShowDialog();
            }

            else Auth.Login(Instance.Token);
        }
    }
}