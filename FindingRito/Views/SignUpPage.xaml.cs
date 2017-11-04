using FindingRito.Models;
using FindingRito.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FindingRito.Views
{
    public partial class SignUpPage : ContentPage
    {
        AzureDataService m_AzureDataService;
        List<Member> m_UsersList;

        public SignUpPage()
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
            var user = new User()
            {
                UserName = usernameEntry.Text,
                Password = passwordEntry.Text,
                Email = emailEntry.Text
            };

            // Sign up logic goes here

            var signUpSucceeded = AreDetailsValid(user);
            if (signUpSucceeded)
            {
                await m_AzureDataService.AddMember(user.UserName, user.Password, user.Email);
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = "Fields cannot be empty";
            }
        }

        bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
        }
    }
}