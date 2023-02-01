using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection.Services.Interfaces
{
    public interface ITaxService
    {
        decimal VatAmount(decimal price, decimal vat);
    }
}
