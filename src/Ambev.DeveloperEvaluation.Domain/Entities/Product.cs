using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }
        public bool Active { get; private set; }

        public Product(string name)
        {
            Name = name;
            Active = true;
        }

        public void Edit(string name)
        {
            Name = name;
        }

        public void Inactive()
        {
            if (!Active)
                throw new InvalidOperationException("Product is already inactive.");

            Active = false;
        }
    }
}
