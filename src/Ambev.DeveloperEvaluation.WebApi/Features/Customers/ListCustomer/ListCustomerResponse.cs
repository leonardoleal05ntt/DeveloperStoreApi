namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomer
{
    public class ListCustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public bool Active { get; set; }
    }
}
