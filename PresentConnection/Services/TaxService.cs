using PresentConnection.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection.Services
{
    public class TaxService : ITaxService
    {
        public decimal VatAmount(decimal price, decimal vat)
        {
            return Math.Round(price * (vat / 100), 2);
        }
    }
}
