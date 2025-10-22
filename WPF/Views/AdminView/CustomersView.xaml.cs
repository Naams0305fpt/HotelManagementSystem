using System.Windows.Controls;
using PhamHuynhSumWPF.ViewModels;

namespace PhamHuynhSumWPF.Views.AdminView
{

    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            InitializeComponent();
            DataContext = new CustomersViewModel();
        }
    }
}