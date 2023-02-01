namespace PresentConnection.Models.Interfaces
{
    public interface ISupplier
    {
        bool VatPayer { get; set; }
        void Invoice(Client client, Product product, int vatRate, decimal vatAmount);
    }
}
