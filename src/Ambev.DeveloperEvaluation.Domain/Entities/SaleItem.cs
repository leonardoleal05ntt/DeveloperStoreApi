using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public Guid SaleId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total => Quantity * UnitPrice * (1 - Discount);
        public Product Product { get; private set; }

        public SaleItem(Guid productId, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            RecalculateDiscount();
        }

        public void Update(int quantity, decimal unitPrice)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            RecalculateDiscount();
        }

        private void RecalculateDiscount()
        {
            if (Quantity >= 10)
                Discount = 0.20m;
            else if (Quantity >= 4)
                Discount = 0.10m;
            else
                Discount = 0.0m;
        }

        private SaleItem() { }
    }
}
