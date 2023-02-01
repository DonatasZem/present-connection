using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection.Enums
{
    public enum AppReturnResults
    {
        SupplierNotVatPayer,
        SupplierIsVatPayerClientNotInEu,
        LivesInEuropeButDifferentCountry,
        BothLivesInSameCountry,
        CountryNotFound,
    }
}
