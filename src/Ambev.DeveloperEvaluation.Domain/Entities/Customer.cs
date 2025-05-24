using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; private set; }
        public string DocumentNumber { get; private set; }
        public bool Active { get; private set; }

        public Customer(string name, string documentNumber)
        {
            Name = name;
            DocumentNumber = documentNumber;
            Active = true;
        }

        public void Inactive() => Active = !Active;
    }
}
