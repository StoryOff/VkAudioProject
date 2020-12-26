using System;
using System.Collections.Generic;
using System.Text;

namespace VkAudioProject.ViewModels
{
    class Settings : JsonSettings
    {
        #region Storage
        private static Settings _instance;
        private string _token;
        #endregion

        public static Settings Instance
        {
            get => _instance ??= new Settings();
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }
    }
}
