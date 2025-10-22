using PhamHuynhSumWPF.ViewModels;
using System.Windows;


namespace PhamHuynhSumWPF.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var vm = new LoginViewModel();
            DataContext = vm;
            vm.RequestClose += (_, __) => this.Close();
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}