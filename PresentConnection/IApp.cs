using PresentConnection.Enums;
using PresentConnection.Models;

namespace PresentConnection
{
    public interface IApp
    {
        AppReturnResults Main(Client client, Supplier supplier, Product product, List<Country> euCountries, List<Country> otherCountries);
    }
}
