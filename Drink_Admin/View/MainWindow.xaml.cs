using Drink_Admin.ViewModel;
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

namespace Drink_Admin.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ModelView();
            checkEdit.Checked += CheckEdit_Checked;
            checkEdit.Unchecked += CheckEdit_Unchecked;
        }

        private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement el in stack.Children)
            {
                if (el is TextBox)
                    (el as TextBox).IsEnabled = false;
                if (el is ComboBox)
                    (el as ComboBox).IsEnabled = false;
            }
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement el in stack.Children)
            {
                if (el is TextBox)
                    (el as TextBox).IsEnabled = true;
                if (el is ComboBox)
                    (el as ComboBox).IsEnabled = true;
            }
        }
    }
}
