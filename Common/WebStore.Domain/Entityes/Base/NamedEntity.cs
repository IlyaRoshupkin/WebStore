using WebStore.Domain.Entityes.Base.Interfaces;

namespace WebStore.Domain.Entityes.Base
{
    public abstract class NamedEntity : Entity, IEntity
    {
        public string Name { get; set; }
    }
}
