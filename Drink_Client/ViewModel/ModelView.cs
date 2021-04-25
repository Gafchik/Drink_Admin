using Drink_Client.Entities;
using Drink_Client.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Drink_Client.ViewModel
{
    namespace Drink_Admin.ViewModel
    {
        public class ModelView : INotifyPropertyChanged
        {
            public ObservableCollection<Drink> Drinks { get; set; }
            private Drink_Repository drink_Repository = new Drink_Repository();

            public ModelView() { InitializeComponent(); }
            public void InitializeComponent()
            {
                if (Drinks != null)
                    Drinks.Clear();
                Drinks = new ObservableCollection<Drink>(drink_Repository.GetColl());
                OnPropertyChanged("Drinks");
            }
            #region PropertyChanged
            public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
            public void OnPropertyChanged([CallerMemberName] string prop = "")
               => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            #endregion

            #region full prop bind

            private Drink selected_item; // выбраный админ для списка

            public Drink Selected_Item
            {
                get { return selected_item; }
                set { selected_item = value; OnPropertyChanged("Selected_Item"); }
            }


            private string serch_str; // строка поиска админа

            public string Serch_str
            {
                get { return serch_str; }
                set
                {
                    serch_str = value; OnPropertyChanged("Serch_srt");
                    if (Drinks != null)
                        GC.Collect(GC.GetGeneration(Drinks));
                    Drinks = new ObservableCollection<Drink>(drink_Repository.GetColl().ToList().FindAll(i => i.Name.ToLower().Contains(serch_str.ToLower())));
                    OnPropertyChanged("Drinks");

                }
            }
            #endregion

           

            #region sell
            private RelayCommand sell; // изменение выбраного админа
            public RelayCommand Sell
            {
                get
                {
                    return sell ?? (sell = new RelayCommand(act =>
                    {
                        try
                        {
                            if (MessageBox.Show("Продать напиток?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                return;
                            Selected_Item.Count_Sell++;
                            drink_Repository.Update(Selected_Item);
                            InitializeComponent();
                            MessageBox.Show("Информация обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Операция не успешна", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }));
                }
            }

            #endregion
        }
    }
}
