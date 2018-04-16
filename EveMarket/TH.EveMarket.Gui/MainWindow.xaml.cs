namespace TH.EveMarket.Gui
{
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using TH.EveMarket.Library;
    using TH.EveMarket.Library.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Market _market;

        public MainWindow()
        {
            InitializeComponent();
            this._market = new Market();

            this.RoutesComboBox.ItemsSource = this._market.Routes;

            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.SelectedItem as Route));
            this.RoutesComboBox.SelectedIndex = 0;

            this.ProductsDataGrid.ItemsSource = this._market.Products;
            this.RoutesDataGrid.ItemsSource = this._market.Routes;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this._market.DownloadMarketData();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
             this._market.Products.Add(Product.CreateProduct(this.AddProductTextBox.Text));
        }

        private void RoutesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.SelectedItem as Route));
        }
    }
}
