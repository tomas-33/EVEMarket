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
        }

        private void SetSources()
        {
            this.RoutesComboBox.ItemsSource = this._market.Routes;

            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.SelectedItem as Route));
            this.RoutesComboBox.SelectedIndex = 0;

            this.ProductsDataGrid.ItemsSource = this._market.Products;
            this.RoutesDataGrid.ItemsSource = this._market.Routes;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this._market.DownloadMarketData();
            this.LastUpdatedTextBlock.Text = $"Last Updated: {this._market.MarketItems.LastUpdated}";
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var n = this._market.Products.OrderBy(p => p.Sequence).FirstOrDefault()?.Sequence;
            int sequence = n != null ? (int)n++ : 0;
            this._market.Products.Add(Product.CreateProduct(this.AddProductTextBox.Text, sequence));
        }

        private void RoutesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.SelectedItem as Route));
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this._market.SaveData();
        }

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this._market.LoadData();
            this.SetSources();
            this.LastUpdatedTextBlock.Text = $"Last Updated: {this._market.MarketItems.LastUpdated}";
        }

        private void MarketDataDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null) return;

            Clipboard.SetText((row.Item as MarketItem).Product.Name);
        }
    }
}
