using Depreciation.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Depreciation
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum DepreciationType
        {
            StraightLine,
            DoubleDeclining
        }
        private ObservableCollection<DepreciationStraightLine> items { get; set; }
        private DepreciationType depreciationType = DepreciationType.StraightLine;


        public MainWindow()
        {
            items = new ObservableCollection<DepreciationStraightLine>();

            InitializeComponent();

            DataContext = this;
            List_Items.ItemsSource = items;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (List_Items.SelectedItem == null)
            {
                MessageBox.Show("Select an item to be removed", "Remove item", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Remove item", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    items.Remove((DepreciationStraightLine)List_Items.SelectedItem);
                }
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            decimal startValue, endValue;
            int lifetime;
            DateTime? inDate, outDate;

            string title = Input_Title.Text;
            if (! decimal.TryParse(Input_StartValue.Text, out startValue))
            {
                MessageBox.Show("StartValue should be a decimal!");
                return;
            }
            if (! decimal.TryParse(Input_EndValue.Text, out endValue))
            {
                MessageBox.Show("EndValue should be a decimal!");
                return;
            }
            if (! int.TryParse(Input_Lifetime.Text, out lifetime))
            {
                MessageBox.Show("Lifetime is an int(number of years)!");
                return;
            }
            inDate = Input_DateIn.SelectedDate;
            outDate = Input_DateOut.SelectedDate;
            if (inDate is null || outDate is null)
            {
                MessageBox.Show("Please select date in and date out!");
                return;
            }
            if (inDate >= outDate)
            {
                MessageBox.Show("Date in should be before date out!");
                return;
            }
           
            DepreciationStraightLine item;
            switch (depreciationType)
            {
                case DepreciationType.StraightLine:
                    items.Add(new DepreciationStraightLine
                    {
                        Title = title,
                        DateAddedToInventory = (DateTime)inDate,
                        DateRemovedFromInventory = (DateTime)outDate,
                        StartValue = startValue,
                        EndValue = endValue,
                        Lifetime = lifetime
                    });
                    break;
                case DepreciationType.DoubleDeclining:
                    items.Add(new DepreciationDoubleDeclining
                    {
                        Title = title,
                        DateAddedToInventory = (DateTime)inDate,
                        DateRemovedFromInventory = (DateTime)outDate,
                        StartValue = startValue,
                        EndValue = endValue,
                        Lifetime = lifetime
                    });
                    break; 
            }

            // MessageBox.Show(items.Last().SalvageValue.ToString());
        }

        private void StraightLine_Checked(object sender, RoutedEventArgs e)
        {
            depreciationType = DepreciationType.StraightLine;
        }

        private void DoubleDeclining_Checked(object sender, RoutedEventArgs e)
        {
            depreciationType = DepreciationType.DoubleDeclining;
        }

        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            decimal totalValue =
                items.Select(i => i.SalvageValue)
                     .Aggregate(0m, (acc, v) => acc + v);

            Label_CalculatedValue.Content =
                $"The total value of your inventory is ${totalValue:0.##}";
        }

        private void List_Items_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (List_Items.SelectedItem == null)
            {
                MessageBox.Show("Select an item to be removed", "Remove item", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                DepreciationStraightLine item = (DepreciationStraightLine)List_Items.SelectedItem;
                Input_Title.Text = item.Title;
                Input_StartValue.Text = item.StartValue.ToString();
                Input_EndValue.Text = item.EndValue.ToString();
                Input_Lifetime.Text = item.Lifetime.ToString();
                Input_DateIn.SelectedDate = item.DateAddedToInventory;
                Input_DateOut.SelectedDate = item.DateRemovedFromInventory;

                if (List_Items.SelectedItem.GetType() == typeof(DepreciationStraightLine))
                {
                    Radio_StraightLine.IsChecked = true;
                    depreciationType = DepreciationType.StraightLine;
                }
                else if (List_Items.SelectedItem.GetType() == typeof(DepreciationDoubleDeclining))
                {
                    Radio_DoubleDeclining.IsChecked = true;
                    depreciationType = DepreciationType.DoubleDeclining;
                }
            }
        }
    }
}
