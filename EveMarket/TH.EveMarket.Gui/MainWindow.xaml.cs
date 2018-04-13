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
            this._market.LoadData();

            var routes = this._market.MarketItems.Select(i => i.Route).Distinct();
            this.RoutesComboBox.ItemsSource = routes;

            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.Items.CurrentItem as Route));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this._market.DownloadMarketData();
            this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.Items.CurrentItem as Route));
        }

        private void RoutesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems.Where(i => i.Route == (this.RoutesComboBox.Items.CurrentItem as Route));
        }
    }
}
