using API.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUi
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window, INotifyPropertyChanged
    {

        private DateTime SelectedDateValue;

        public ObservableCollection<Car> Cars { get; set; }

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
        private readonly Car CarValue;


        private Car SelectedCarValue;
        private MainWindow _parent;

        public Car SelectedCar
        {
            get => SelectedCarValue;
            set
            {
                SelectedCarValue = value;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BookingWindow(MainWindow parent, ObservableCollection<Car> cars)
        {
            Cars = cars;
            InitializeComponent();
            DataContext = this;
            _parent = parent;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BookCar();
        }
        private async Task BookCar()
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(SelectedDate), Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await http.PostAsync("http://localhost:8080/cars/" + SelectedCar.Id, content);
            resp.EnsureSuccessStatusCode();
            _parent.FetchCars();
            Close();
        }
    }
}
