using BusinessLayer.Services;
using Models.Entities;
using PhamHuynhSumWPF.ViewModels;
using System.Windows;

namespace PhamHuynhSumWPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : Window
    {
        public CustomerService? CustomerService { get; set; }
        public bool IsEditMode { get; set; } = false;
        public EditCustomerWindow()
        {
            InitializeComponent();
        }

        // --- PHƯƠNG THỨC BỊ THIẾU ---
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ViewModels.EditCustomerViewModel vm || CustomerService == null)
            {
                MessageBox.Show("Error: Service not initialized.");
                return;
            }

            DataAccessLayer.Common.RepositoryResult<Customer> res;
            if (IsEditMode)
            {
                res = CustomerService.Update(vm.Entity);
            }
            else
            {
                res = CustomerService.Create(vm.Entity); // Logic gọi nằm ở đây
            }

            if (res.Success)
            {
                this.DialogResult = true; // Chỉ đóng nếu thành công
            }
            else
            {
                MessageBox.Show(res.Error); // Hiển thị lỗi và KHÔNG đóng
            }
        }

        // --- PHƯƠNG THỨC BỊ THIẾU ---
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}