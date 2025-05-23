﻿using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public Guid CustomerId { get; private set; }
        public Guid BranchId { get; private set; }
        public bool IsCancelled { get; private set; }
        public List<SaleItem> Items { get; private set; }
        public decimal Total => Items.Sum(x => x.Total);

        public Sale(string saleNumber, Guid customerId, Guid branchId)
        {
            SaleNumber = saleNumber;
            CustomerId = customerId;
            BranchId = branchId;
        }

        public void Cancel() => IsCancelled = !IsCancelled;
    }
}
