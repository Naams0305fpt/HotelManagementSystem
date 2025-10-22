using System.Windows.Controls;
namespace PhamHuynhSumWPF.Views.AdminView
{
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ReportViewModel();
        }
    }
}