using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VkAudioProject.Commands;
using VkNet.AudioBypassService.Exceptions;
using VkNet.Exception;

namespace VkAudioProject.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        #region storage
        // Visibility
        private bool _isLogPassVisible = true;
        private bool _isOkBtnActive = true;
        private bool _isTwoAuthVisible;

        // TextBox
        private string _login;
        private string _twoAuthCode;


        #endregion

        #region Visibility
        public bool IsLogPassVisible
        {
            get => _isLogPassVisible;
            set => SetProperty(ref _isLogPassVisible, value);
        }

        public bool IsOkBtnActive
        {
            get => _isOkBtnActive;
            set => SetProperty(ref _isOkBtnActive, value);
        }

        public bool IsTwoAuthVisible
        {
            get => _isTwoAuthVisible;
            set => SetProperty(ref _isTwoAuthVisible, value);
        }
        #endregion

        #region TextBox
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string TwoAuthCode
        {
            get => _twoAuthCode;
            set => SetProperty(ref _twoAuthCode, value);
        }
        #endregion

        public Action CloseAction { get; set; }

        public RelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(obj => Authorize(obj as PasswordBox));
        }

        private async void Authorize(PasswordBox passwordBox)
        {
            string password = passwordBox?.Password;

            IsOkBtnActive = false;

            try
            {
                if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(password) || Login.Length <= 4 || password.Length <= 5)
                {
                    MessageBox.Show("Введите Логин и Пароль");
                    return;
                }

                // логин с двухфакторной авторизацей
                if (IsTwoAuthVisible)
                {
                    if (string.IsNullOrEmpty(TwoAuthCode) || TwoAuthCode.Length <= 3)
                    {
                        MessageBox.Show("Введите код Двойной Аутентификации");
                        return;
                    }

                    await Auth.Login(Login, password, TwoAuthCode);
                }
                // логин по логину и паролю
                else
                {
                    await Auth.Login(Login, password);
                }
            }
            catch (VkAuthException)
            {
                MessageBox.Show(IsTwoAuthVisible ? "Неправильно введен код Двойной Аутентификации" : "Неправильно введен Логин или Пароль",
                    "Error");

                IsOkBtnActive = true;
            }
            catch (CaptchaNeededException)
            {
                MessageBox.Show("Требуется ввести капчу");
            }
            catch (InvalidOperationException)
            {
                IsLogPassVisible = false;
                IsTwoAuthVisible = true;
            }

            finally
            {
                if (Auth.Api.IsAuthorized)
                {
                    CloseAction();
                }
            }
        }
    }
}
