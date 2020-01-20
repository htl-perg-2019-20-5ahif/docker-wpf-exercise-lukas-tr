using API.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace WpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();

        private DateTime SelectedDateValue;

        public event PropertyChangedEventHandler PropertyChanged;


        private static readonly HttpClient http = new HttpClient() { };

        public DateTime SelectedDate
        {
            get => SelectedDateValue;
            set
            {
                SelectedDateValue = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FetchCars();
        }

        public async Task FetchCars(DateTime? date = null)
        {
            HttpResponseMessage resp;
            if (date.HasValue)
            {
                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                query["date"] = date.ToString();
                string queryString = query.ToString();
                resp = await http.GetAsync("http://localhost:8080/cars?" + queryString);
            }
            else
            {
                resp = await http.GetAsync("http://localhost:8080/cars");
            }
            resp.EnsureSuccessStatusCode();

            string responseBody = await resp.Content.ReadAsStringAsync();
            List<Car> cars = JsonSerializer.Deserialize<List<Car>>(responseBody);
            Cars.Clear();
            cars.ForEach(c => Cars.Add(c));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FetchCars();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new BookingWindow(this, Cars).Show();
        }

        private void Grid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Bookings" || e.PropertyName == "DisplayName")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FetchCars(SelectedDate);
        }
    }
}
