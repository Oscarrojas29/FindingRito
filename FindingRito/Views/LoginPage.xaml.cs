using FindingRito.Models;
using FindingRito.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FindingRito.Views
{
    public partial class LoginPage : ContentPage
    {
        AzureDataService m_AzureDataService;
        List<Member> m_UsersList;

        public LoginPage()
        {
            InitializeComponent();
            m_AzureDataService = new AzureDataService();
            m_UsersList = new List<Member>();
            ConnectWithBackEnd();
        }

        private async void ConnectWithBackEnd()
        {
            await m_AzureDataService.Initialize();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                UserName = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            var member = await m_AzureDataService.GetMemberByUserNameAndPassword(user.UserName, user.Password);
            if (member.Any())
            {
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }
    }
}