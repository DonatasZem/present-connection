using PresentConnection;
using PresentConnection.Enums;
using PresentConnection.Models;
using PresentConnection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnectionTests
{
    public class AppTests
    {
        private readonly IApp _app;

        public AppTests()
        {
            _app = new App(new TaxService());
        }

        [Fact]
        public void When_Supplier_IsVatPayer_ClientNotInEu()
        {
            List<Country> euCountries = new List<Country>
            {
                new Country { Name = "Lithuania", VatRate = 21},
            };

            List<Country> otherCountries = new List<Country>
            {
                new Country {Name = "Algeria", VatRate = 1}
            };

            var client = new Client(1, "John Doe", new Location { Country = "Algeria", City = "Algebra" }, false);
            var supplier = new Supplier(1, "Raimio Servisas", new Location { Country = "Lithuania", City = "Kaunas" }, true);
            var product = new Product { Id = 1, Name = "Sankabos komplektas", Price = 155.88M };

            var result = _app.Main(client, supplier, product, euCountries, otherCountries);

            Assert.Equal(AppReturnResults.SupplierIsVatPayerClientNotInEu, result);
        }

        [Fact]
        public void When_Country_NotFound()
        {
            List<Country> euCountries = new List<Country>
            {
                new Country { Name = "Lithuania", VatRate = 21},
            };

            List<Country> otherCountries = new List<Country>
            {
                new Country {Name = "Algeria", VatRate = 1}
            };

            var client = new Client(1, "Jonas", new Location { Country = "Lithuania", City = "Kaunas" }, false);
            var supplier = new Supplier(1, "Raimio Servisas", new Location { Country = "Lit", City = "Kaunas" }, false);
            var product = new Product { Id = 1, Name = "Sankaba", Price = 155.88M };

            var result = _app.Main(client, supplier, product, euCountries, otherCountries);

            Assert.Equal(AppReturnResults.CountryNotFound, result);
        }

        [Fact]
        public void When_Supplier_IsVatPayer_Client_LivesInEuropeBut_DifferentCountry()
        {
            List<Country> euCountries = new List<Country>
            {
                new Country { Name = "Lithuania", VatRate = 21},
                new Country { Name = "Germany", VatRate = 19},
            };

            List<Country> otherCountries = new List<Country>
            {
                new Country {Name = "Algeria", VatRate = 1}
            };

            var client = new Client(1, "Jonas Jonaitis", new Location { Country = "Germany", }, false);
            var supplier = new Supplier(1, "Raimio Servisas", new Location { Country = "Lithuania", City = "Kaunas" }, true);
            var product = new Product { Id = 1, Name = "Sankabos komplektas", Price = 155.88M };

            var result = _app.Main(client, supplier, product, euCountries, otherCountries);

            Assert.Equal(AppReturnResults.LivesInEuropeButDifferentCountry, result);
        }

        [Fact]
        public void When_Supplier_IsVatPayer_BothLivingInSameCountry()
        {
            List<Country> euCountries = new List<Country>
            {
                new Country { Name = "Lithuania", VatRate = 21},
                new Country { Name = "Germany", VatRate = 19},
            };

            List<Country> otherCountries = new List<Country>
            {
                new Country {Name = "Algeria", VatRate = 1}
            };

            var client = new Client(1, "Algirdas", new Location { Country = "Algeria", }, false);
            var supplier = new Supplier(1, "Raimio Servisas", new Location { Country = "Algeria" }, true);
            var product = new Product { Id = 1, Name = "Sankabos komplektas", Price = 155.88M };

            var result = _app.Main(client, supplier, product, euCountries, otherCountries);

            Assert.Equal(AppReturnResults.BothLivesInSameCountry, result);
        }


        [Fact]
        public void When_Supplier_IsNotVatPayer()
        {
            List<Country> euCountries = new List<Country>
            {
                new Country { Name = "Lithuania", VatRate = 21},
            };

            List<Country> otherCountries = new List<Country>
            {
                new Country {Name = "Algeria", VatRate = 1}
            };

            var client = new Client(1, "Petras Petraitis", new Location { Country = "Lithuania", City = "Kaunas" }, false);
            var supplier = new Supplier(1, "Raimio Servisas", new Location { Country = "Lithuania", City = "Kaunas" }, false);
            var product = new Product { Id = 1, Name = "Sankaba", Price = 155.88M };

            var result = _app.Main(client, supplier, product, euCountries, otherCountries);

            Assert.Equal(AppReturnResults.SupplierNotVatPayer, result);
        }




    }
}
