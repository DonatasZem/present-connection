using DocumentFormat.OpenXml.Packaging;
using PresentConnection.Models.Interfaces;
using System.Reflection;

namespace PresentConnection.Models
{
    public class Supplier : IBaseModel, ISupplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public bool IsJuridical { get; set; } = true;
        public bool VatPayer { get; set; }

        public Supplier(int id, string name, Location location, bool vatPayer)
        {
            this.Id = id;
            this.Name = name;
            this.Location = location;
            this.VatPayer = vatPayer;
        }
        public void Invoice(Client client, Product product, int vatRate, decimal vatAmount)
        {
            var templatePath = Path.Combine((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.Split(new string[] { "/bin" }, StringSplitOptions.None)[0], @"Templates\Invoice.docx");

            byte[] byteArray = File.ReadAllBytes(templatePath);

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, (int)byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    wordDocument.MainDocumentPart.Document.Body.InnerXml = body.InnerXml
                        .Replace("CompanyNamechanged", $"{this.Name}")
                        .Replace("CompanyCitychanged", $"{this.Location.Country}, {this.Location.City}")
                        .Replace("ClientNamechanged", $"{client.Name}")
                        .Replace("ProductNameChanged", $"{product.Name}")
                        .Replace("ClientAddresschanged", $"{client.Location.Country}, {client.Location.City}")
                        .Replace("SubTotalChanged", $"{product.Price}")
                        .Replace("PriceTotalChanged", $"{product.Price}")
                        .Replace("TaxRateChanged", $"{vatRate}%")
                        .Replace("TotalTaxChanged", $"{vatAmount}")
                        .Replace("BalanceDueChanged", $"{product.Price + vatAmount}");

                    wordDocument.Close();
                }

                File.WriteAllBytes(Path.Combine((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.Split(new string[] { "/bin" }, StringSplitOptions.None)[0], $@"Invoices\{client.Name}.docx"), stream.ToArray());
            }
        }
    }
}
