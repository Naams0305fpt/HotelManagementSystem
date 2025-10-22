using System.Windows.Controls;
namespace PhamHuynhSumWPF.Views.AdminView
{
    public partial class RoomsView : UserControl
    {
        public RoomsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.RoomsViewModel();
        }
    }
}