namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer
{
    public class GetCustomerResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public bool Active { get; set; }
    }
}
