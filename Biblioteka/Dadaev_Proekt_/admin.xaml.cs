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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Dadaev_Proekt_
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public PlotModel PlotModel { get; private set; }
        public admin()
        {
            InitializeComponent();
            SetupModel();
            DataContext = this;
        }
        private void SetupModel()
        {
            PlotModel = new PlotModel { Title = "Количество выданных книг по дням" };
            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White
            };

            // Пример данных
            lineSeries.Points.Add(new DataPoint(1, 3));
            lineSeries.Points.Add(new DataPoint(2, 5));
            // Добавьте реальные данные здесь

            PlotModel.Series.Add(lineSeries);
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, Title = "День" });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, MaximumPadding = 0.1, Title = "Количество книг" });
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var readerWindow = new user();
            readerWindow.Show();
        }

        private void add(object sender, RoutedEventArgs e)
        {
            var readerWindow = new adduser();
            readerWindow.Show();
        }

        private void addbook(object sender, RoutedEventArgs e)
        {
            var readerWindow = new addbook();
            readerWindow.Show();
        }
    }
}
