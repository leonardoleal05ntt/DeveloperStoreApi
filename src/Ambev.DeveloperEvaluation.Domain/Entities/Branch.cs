using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; private set; }
        public bool Active { get; private set; }

        public Branch(string name)
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
                throw new InvalidOperationException("Branch is already inactive.");

            Active = false;
        }
    }
}
