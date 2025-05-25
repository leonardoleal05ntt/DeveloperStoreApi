using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public Guid CustomerId { get; private set; }
        public Guid BranchId { get; private set; }
        public bool IsCancelled { get; private set; }
        public List<SaleItem> Items { get; private set; } = new List<SaleItem>();
        public decimal Total => Items.Sum(x => x.Total);
        public Customer Customer { get; private set; }
        public Branch Branch { get; private set; }

        public Sale(string saleNumber, Guid customerId, Guid branchId)
        {
            SaleNumber = saleNumber;
            CustomerId = customerId;
            BranchId = branchId;
        }

        public void AddItem(SaleItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(SaleItem item)
        {
            Items.Remove(item);
        }

        public void Cancel() => IsCancelled = !IsCancelled;
    }
}
