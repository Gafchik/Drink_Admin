

using Drink_Admin.Entities;
using Drink_Admin.View;
using Drink_Admin.View.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

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

        #region new category
        private RelayCommand new_category; // открыть окно  с админами
        public RelayCommand New_category
        {
            get { return new_category ?? (new_category = new RelayCommand(act => { new New_Drink().ShowDialog(); InitializeComponent(); })); }
        }
        private RelayCommand cansel_new; // отмена  создания нового админа
        public RelayCommand Cansel_new
        {
            get { return cansel_new ?? (cansel_new = new RelayCommand(act => { (act as Window).Close(); })); }
        }
        internal void Add_new(string name, string prise, string amount, Window window) //кнопка добавления нового админа
        {
            if (MessageBox.Show("Добавить категорию?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            if (name == "" && prise == "" && amount == "")
            { MessageBox.Show("Поле не заполнено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            drink_Repository.Create(new Drink
            {
                Name = name,
                Price = float.Parse(prise),
                Amount = float.Parse(amount),
                Count_Sell = 0
            });
            window.Close();
            OnPropertyChanged("Drinks");
        }


        #endregion

        #region edit category
        private RelayCommand edit; // изменение выбраного админа
        public RelayCommand Edit
        {
            get
            {
                return edit ?? (edit = new RelayCommand(act =>
                {
                    try
                    {
                        drink_Repository.Update(Selected_Item);
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

        #region dell category

        private RelayCommand dell; // удаление выбраного админа
        public RelayCommand Dell
        {
            get
            {
                return dell ?? (dell = new RelayCommand(act =>
                {
                    try
                    {
                        if (MessageBox.Show("Удалить категорию?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            return;
                        if (Selected_Item != null)
                            drink_Repository.Delete(Selected_Item);
                        else
                            MessageBox.Show("Нужно выбрать что удалять", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);


                        if (Drinks != null)
                            GC.Collect(GC.GetGeneration(Drinks));
                        Drinks = new ObservableCollection<Drink>(drink_Repository.GetColl());

                        OnPropertyChanged("Drinks");
                        MessageBox.Show("Информация удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Нельзя удалить категорию в которой есть продукты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
