using PresentConnection.Enums;
using PresentConnection.Models;
using PresentConnection.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection
{
    public class App : IApp
    {
        private readonly ITaxService _taxService;

        public App(ITaxService taxService)
        {
            _taxService = taxService;
        }

        public AppReturnResults Main(Client client, Supplier supplier, Product product, List<Country> euCountries, List<Country> otherCountries)
        {
            bool clientLivesInEu = LivesIn(client.Location.Country, euCountries);
            bool supplierLivesInEu = LivesIn(supplier.Location.Country, euCountries);
            bool clientLivesInOtherCountry = LivesIn(client.Location.Country, otherCountries);
            bool supplierLivesInOtherCountry = LivesIn(supplier.Location.Country, otherCountries);

            if (clientLivesInEu == false && clientLivesInOtherCountry == false) return AppReturnResults.CountryNotFound;
            if (supplierLivesInEu == false && supplierLivesInOtherCountry == false) return AppReturnResults.CountryNotFound;

            if (supplier.VatPayer == false)
            {
                supplier.Invoice(client, product, 0, 0);

                return AppReturnResults.SupplierNotVatPayer;
            }

            if (supplier.VatPayer == true && clientLivesInEu == true && supplierLivesInEu == true && supplier.Location.Country != client.Location.Country)
            {
                var vatRate = euCountries.FirstOrDefault(c => c.Name == supplier.Location.Country);

                var vatAmount = _taxService.VatAmount(product.Price, vatRate.VatRate);

                supplier.Invoice(client, product, vatRate.VatRate, vatAmount);

                return AppReturnResults.LivesInEuropeButDifferentCountry;
            }

            if (supplier.VatPayer == true && client.Location.Country == supplier.Location.Country)
            {
                Country vatRate = null;

                if(supplierLivesInEu == true)
                {
                    vatRate = euCountries.FirstOrDefault(c => c.Name == supplier.Location.Country);
                } else
                {
                    vatRate = otherCountries.FirstOrDefault(c => c.Name == supplier.Location.Country);
                }

                var vatAmount = _taxService.VatAmount(product.Price, vatRate.VatRate);

                supplier.Invoice(client, product, vatRate.VatRate, vatAmount);

                return AppReturnResults.BothLivesInSameCountry;
            }

            if (supplier.VatPayer == true && clientLivesInEu == false)
            {
                supplier.Invoice(client, product, 0, 0);

                return AppReturnResults.SupplierIsVatPayerClientNotInEu;
            }

            return AppReturnResults.CountryNotFound;
        }

        private bool LivesIn(string country, List<Country> countries)
        {
            var result = countries.FirstOrDefault(c => c.Name == country);

            if (result == null) return false;

            return true;
        }
    }
}
