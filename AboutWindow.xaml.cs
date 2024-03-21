using System.Diagnostics;
using System.Windows;

namespace BowieD.Unturned.IDTableGenerator
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var p = new ProcessStartInfo(e.Uri.ToString())
            {
                UseShellExecute = true
            };

            Process.Start(p);
        }
    }
}
