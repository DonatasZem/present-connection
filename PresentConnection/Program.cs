// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using PresentConnection;
using PresentConnection.Models;
using PresentConnection.Models.ApiModel;
using PresentConnection.Services;
using PresentConnection.Services.Interfaces;
using System.Net.Http.Headers;

List<Country> euCountries = new List<Country>
{
    new Country { Name = "Austria", VatRate = 20},
    new Country { Name = "Belgium", VatRate = 21},
    new Country { Name = "Bulgaria", VatRate = 20},
    new Country { Name = "Croatia", VatRate = 25},
    new Country { Name = "Republic of Cyprus", VatRate = 19},
    new Country { Name = "Czech Republic", VatRate = 21},
    new Country { Name = "Denmark", VatRate = 25},
    new Country { Name = "Estonia", VatRate = 20},
    new Country { Name = "Finland", VatRate = 24},
    new Country { Name = "France", VatRate = 20},
    new Country { Name = "Germany", VatRate = 19},
    new Country { Name = "Greece", VatRate = 24},
    new Country { Name = "Hungary", VatRate = 27},
    new Country { Name = "Ireland", VatRate = 23},
    new Country { Name = "Italy", VatRate = 22},
    new Country { Name = "Latvia", VatRate = 21},
    new Country { Name = "Lithuania", VatRate = 21},
    new Country { Name = "Luxembourg", VatRate = 17},
    new Country { Name = "Malta", VatRate = 18},
    new Country { Name = "Netherlands", VatRate = 21},
    new Country { Name = "Poland", VatRate = 23},
    new Country { Name = "Portugal", VatRate = 23},
    new Country { Name = "Romania", VatRate = 19},
    new Country { Name = "Slovakia", VatRate = 20},
    new Country { Name = "Slovenia", VatRate = 22},
    new Country { Name = "Spain", VatRate = 21},
    new Country { Name = "Sweden", VatRate = 25},
};

List<Country> otherCountries = await CountriesApi();

var client = new Client(1, "Petras", new Location { Country = "Lithuania", City = "Kaunas" }, false);
var supplier = new Supplier(1, "Raimio Servisas", new Location { Country = "Lithuania", City = "Kaunas" }, true);
var product = new Product { Id = 1, Name = "Sankaba", Price = 155.88M };

ITaxService taxService = new TaxService();

IApp app = new App(taxService);

app.Main(client, supplier, product, euCountries, otherCountries);












































//string fullFilePath = Path.Combine((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.Split(new string[] { "/bin" }, StringSplitOptions.None)[0], "eucountries.txt");
//string newFile = Path.Combine((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.Split(new string[] { "/bin" }, StringSplitOptions.None)[0], "next.txt");

//using (var sr = new StreamReader(fullFilePath))
//{
//    var fileInfo = sr.ReadToEnd();
//    var fileArr = fileInfo.Split(",");
//    var text = "";

//    foreach (var item in fileArr)
//    {
//        text += "new Country " + "{" + $" Name = \"{item}\", VatRate = 1" + "}, \n";
//    }

//    using(var file = File.Create(newFile))
//    {
//        byte[] info = new UTF8Encoding(true).GetBytes(text);
//        file.Write(info, 0, info.Length);
//    }
//}

async Task<List<Country>> CountriesApi()
{
    using (var client = new HttpClient())
    {
        List<Country> result = new List<Country>();

        client.BaseAddress = new Uri("https://api.first.org/data/v1/countries");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = await client.GetAsync("https://api.first.org/data/v1/countries");

        if (response.IsSuccessStatusCode)
        {
            var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var rawResponse = readTask.GetAwaiter().GetResult();
            var deResponse = JsonConvert.DeserializeObject<ApiCall>(rawResponse);

            foreach (var prop in deResponse.Data.GetType().GetProperties())
            {
                var country = prop.GetValue(deResponse.Data).GetType().GetProperty("Country").GetValue(prop.GetValue(deResponse.Data));
                var countryString = country.ToString();

                result.Add(new Country { Name = countryString, VatRate = 1 });
            }

            return result;
        }

        return null;
    }
}
