using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Frapid.NPoco
{
    public class MapperCollection : List<IMapper>
    {
        internal readonly Dictionary<Type, ObjectFactoryDelegate> Factories = new Dictionary<Type, ObjectFactoryDelegate>();
        public delegate object ObjectFactoryDelegate(DbDataReader dataReader);

        public MapperCollection()
        {
#if !NET35
            this.Factories.Add(typeof(object), x => new PocoExpando());
#endif
            this.Factories.Add(typeof(IDictionary<string, object>), x => new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
            this.Factories.Add(typeof(Dictionary<string, object>), x => new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
        }

        public void RegisterFactory<T>(Func<DbDataReader, T> factory)
        {
            this.Factories[typeof(T)] = x => factory(x);
        }

        public ObjectFactoryDelegate GetFactory(Type type)
        {
            return this.Factories.ContainsKey(type) ? this.Factories[type] : null;
        }

        public bool HasFactory(Type type)
        {
            return this.Factories.ContainsKey(type);
        }

        public void ClearFactories(Type type = null)
        {
            if (type != null)
            {
                this.Factories.Remove(type);
            }
            else
            {
                this.Factories.Clear();
            }
        }

        public Func<object, object> Find(Func<IMapper, Func<object, object>> predicate)
        {
            return this.Select(predicate).FirstOrDefault(x => x != null);
        }

        public object FindAndExecute(Func<IMapper, Func<object, object>> predicate, object value)
        {
            Func<object, object> converter = this.Find(predicate);
            return converter != null ? converter(value) : value;
        }
    }
}