using Sklepinternetowy.Model;

namespace Sklepinternetowy
{
    public partial class MainPage : ContentPage
    {
        public List<Produkt> Produkty { get; set; } = new List<Produkt>();

        public MainPage()
        {
            InitializeComponent();
            LoadData();
            BindingContext = this;
        }
        private void LoadData()
        {
            var stream = FileSystem.OpenAppPackageFileAsync("data_store.txt").Result;
            using var reader = new StreamReader(stream);

            bool firstLine = true;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                var parts = line.Split(';');

                Produkty.Add(new Produkt
                {
                    Nazwa = parts[1],
                    Obrazek = parts[2],
                    Cena = decimal.Parse(parts[3]),
                    Kategoria = parts[4],
                    Ilość = int.Parse(parts[5])
                });
            }
        }
        private void BuyClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var produkt = (button.BindingContext as Produkt);

            if (produkt.Ilość > 0)
            {
                produkt.Ilość--;
            }

            if (produkt.Ilość == 0)
            {
                DisplayAlert("Brak w magazynie",
                    $"Nie można kupić produktu {produkt.Nazwa}.",
                    "OK");
            }
            BindingContext = null;
            BindingContext = this;
        }
        private void ReturnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var produkt = (button.BindingContext as Produkt);

            produkt.Ilość++;

            BindingContext = null;
            BindingContext = this;
        }
    }
}
