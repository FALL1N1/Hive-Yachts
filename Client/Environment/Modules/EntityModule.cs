using Hive.Client.Environment.Entities;

namespace Hive.Client.Environment.Entities.Modules
{
    public abstract class EntityModule
    {
        public HiveEntity Entity { get; set; }
        public int Id { get; set; }

        public void CallBeginOperation(HiveEntity entity, int id)
        {
            Entity = entity;
            Id = id;

            Begin(entity, id);
        }

        public T As<T>() where T : EntityModule
        {
            return (T)this;
        }

        protected abstract void Begin(HiveEntity entity, int id);
    }
}