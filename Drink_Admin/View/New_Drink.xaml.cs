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
    /// Логика взаимодействия для New_Drink.xaml
    /// </summary>
    public partial class New_Drink : Window
    {
        public New_Drink()
        {
            InitializeComponent();
            DataContext = new ModelView();
            Add.Click += Add_Click;
        }

        private void Add_Click(object sender, RoutedEventArgs e) => (DataContext as ModelView).Add_new(Name.Text, prise.Text, amout.Text, this);
    }
}
