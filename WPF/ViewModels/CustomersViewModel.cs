// --- Thay đổi Using ---
using DataAccessLayer.Repositories.Implementations; // Cần giữ lại để khởi tạo repo
using BusinessLayer.Services; // <<< THÊM USING SERVICE
using Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;
using System.Linq; // Cần cho ToList()
using PhamHuynhSumWPF.Views;

namespace PhamHuynhSumWPF.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        // --- Thay đổi Repo thành Service ---
        // private readonly CustomerRepository _repo = new(); // <<< BỎ DÒNG NÀY
        private readonly CustomerService _service; // <<< THAY BẰNG SERVICE

        public ObservableCollection<Customer> Customers { get; } = [];
        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer { get => _selectedCustomer; set => Set(ref _selectedCustomer, value); }

        private string _keyword = string.Empty;
        public string Keyword { get => _keyword; set { Set(ref _keyword, value); Search(); } }


        public RelayCommand SearchCommand => new(_ => Search());
        public RelayCommand AddCommand => new(_ => Add());
        public RelayCommand EditCommand => new(_ => Edit(), _ => SelectedCustomer != null);
        public RelayCommand DeleteCommand => new(_ => Delete(), _ => SelectedCustomer != null);


        public CustomersViewModel()
        {
            var repo = new CustomerRepository();
            _service = new CustomerService(repo);
            Load();
        }

        private void Load()
        {
            Customers.Clear();
            // Gọi service để tải
            foreach (var c in _service.GetAll()) Customers.Add(c);
        }

        private void Search()
        {
            Customers.Clear();
            var results = _service.Search(Keyword); // <<< GỌI SERVICE ĐỂ TÌM KIẾM
            foreach (var c in results) Customers.Add(c);
        }

        private void Add()
        {
            var vm = new EditCustomerViewModel();
            var dlg = new Views.Dialogs.EditCustomerWindow
            {
                DataContext = vm,
                CustomerService = _service, // Chỉ truyền service vào
                IsEditMode = false
            };

            if (dlg.ShowDialog() == true) // Nếu cửa sổ dialog tự nó báo thành công
            {
                Load(); // Thì chỉ cần tải lại
            }
        }

        private void Edit()
        {
            if (SelectedCustomer == null) return;
            var vm = new EditCustomerViewModel(SelectedCustomer);
            var dlg = new Views.Dialogs.EditCustomerWindow
            {
                DataContext = vm,
                CustomerService = _service,
                IsEditMode = true
            };

            // Chỉ cần Load() khi dialog trả về true.
            // Toàn bộ logic Update đã được chuyển vào EditCustomerWindow.xaml.cs
            if (dlg.ShowDialog() == true)
            {
                Load();
            }
        }

        private void Delete()
        {
            if (SelectedCustomer == null) return;
            if (MessageBox.Show($"Delete {SelectedCustomer.CustomerFullName}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Gọi service để xóa (Delete)
                var res = _service.Delete(SelectedCustomer.CustomerID); // <<< SỬ DỤNG SERVICE
                if (!res.Success)
                {
                    MessageBox.Show(res.Error);
                }
                Load(); // Tải lại danh sách
            }
        }
    }
}