using System;
using System.Collections;
using System.Collections.Generic;

namespace Frapid.NPoco
{
    #if !NET35
    public class PocoExpando : System.Dynamic.DynamicObject, IDictionary<string, object>
    {
        private readonly IDictionary<string, object> Dictionary =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public void Add(KeyValuePair<string, object> item)
        {
            this.Dictionary.Add(item);
        }

        public void Clear()
        {
            this.Dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.Dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.Dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.Dictionary.Remove(item);
        }

        public int Count => this.Dictionary.Keys.Count;

        public bool IsReadOnly => this.Dictionary.IsReadOnly;

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            if (this.Dictionary.ContainsKey(binder.Name))
            {
                result = this.Dictionary[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {
            if (!this.Dictionary.ContainsKey(binder.Name))
                this.Dictionary.Add(binder.Name, value);
            else
                this.Dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(System.Dynamic.InvokeMemberBinder binder, object[] args, out object result)
        {
            if (this.Dictionary.ContainsKey(binder.Name) && this.Dictionary[binder.Name] is Delegate)
            {
                Delegate del = this.Dictionary[binder.Name] as Delegate;
                result = del.DynamicInvoke(args);
                return true;
            }
            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryDeleteMember(System.Dynamic.DeleteMemberBinder binder)
        {
            if (this.Dictionary.ContainsKey(binder.Name))
            {
                this.Dictionary.Remove(binder.Name);
                return true;
            }
            return base.TryDeleteMember(binder);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return this.Dictionary.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            this.Dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return this.Dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.Dictionary.TryGetValue(key, out value);
        }

        public object this[string key]
        {
            get { return this.Dictionary[key]; }
            set { this.Dictionary[key] = value; }
        }

        public ICollection<string> Keys => this.Dictionary.Keys;

        public ICollection<object> Values => this.Dictionary.Values;
    }
    #endif
}