using System.Collections.Generic;
using System.Dynamic;

namespace Frapid.Mapper.Types
{
    public class FrapidDynamic : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary;

        public FrapidDynamic(Dictionary<string, object> dictionary)
        {
            this._dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this._dictionary.TryGetValue(binder.Name, out result);
        }

        public Dictionary<string, object> GetDictionary()
        {
            return this._dictionary;
        }

        public object GetPropertyValue(string name)
        {
            var instance = this.GetDictionary();

            return instance[name];
        }

        public object this[string key] => this.GetPropertyValue(key);

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this._dictionary[binder.Name] = value;

            return true;
        }
    }
}