using System.Collections.Generic;

namespace Frapid.NPoco
{
    public class ExpandoColumn : PocoColumn
    {
        public override void SetValue(object target, object val)
        {
            ((IDictionary<string, object>) target)[this.ColumnName] = val;
        }

        public override object GetValue(object target) 
        { 
            object val=null;
            ((IDictionary<string, object>) target).TryGetValue(this.ColumnName, out val);
            return val;
        }

        public override object ChangeType(object val) { return val; }
    }
}