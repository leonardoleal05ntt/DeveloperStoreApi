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

        public void Inactive() => Active = !Active;
    }
}
