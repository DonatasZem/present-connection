using PresentConnection.Services;
using PresentConnection.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnectionTests
{
    public class TaxServiceTests
    {
        private readonly ITaxService _taxService;

        public TaxServiceTests() 
        {
            _taxService = new TaxService();
        }

        [Fact]
        public void When_CalculatingVatAmount_ExpectingCorrect()
        {
            decimal result = _taxService.VatAmount(120.20M, 20);

            Assert.Equal(24.04M, result);
        }
    }
}
