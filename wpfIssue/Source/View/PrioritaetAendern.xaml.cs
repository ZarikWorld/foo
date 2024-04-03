using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpfIssues.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PrioritaetAendern : Window
    {
        public string ResponseText { get; set; }

        public PrioritaetAendern(string defaultText = "")
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
            InputTextBox.Text = defaultText;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ResponseText = InputTextBox.Text;
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();
        }

        public static int getNewPrioPunkteFromDialog()
        {
            PrioritaetAendern dialog = new PrioritaetAendern("");

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                if (int.TryParse(dialog.ResponseText, out int result) || result > 0)
                {
                    return result;
                }
                else
                {
                    MessageBox.Show("Der eingegebene Wert ist ungültig. Bitte geben Sie eine gültige Zahl ein.", "Ungültiger Wert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            return 0;
        }
    }
}
