namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer
{
    public class GetCustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public bool Active { get; set; }
    }
}
