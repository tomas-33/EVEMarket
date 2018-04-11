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

namespace TH.EveMarket.Gui
{
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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            this._market.LoadData();
            this._market.DownloadMarketData();
            this._market.Calculate();

            this.MarketDataDataGrid.ItemsSource = this._market.MarketItems;
            
        }
    }
}
