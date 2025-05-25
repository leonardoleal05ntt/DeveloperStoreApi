namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.EditCustomer
{
    public class EditCustomerRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
    }
}
