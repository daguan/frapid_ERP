using System;
using System.Collections.Generic;
using System.Linq;

namespace Frapid.NPoco
{
    public static class Snapshotter
    {
        public static Snapshot<T> StartSnapshot<T>(this IDatabase d, T obj)
        {
            return new Snapshot<T>(d.PocoDataFactory.ForType(obj.GetType()), obj);
        }

        public static int Update<T>(this IDatabase d, T obj, Snapshot<T> snapshot)
        {
            return d.Update(obj, snapshot.UpdatedColumns());
        }
    }

    public class Snapshot<T>
    {
        private readonly PocoData _pocoData;
        private T _trackedObject;
        private readonly Dictionary<PocoColumn, object> _originalValues = new Dictionary<PocoColumn, object>();

        public Snapshot(PocoData pocoData, T trackedObject)
        {
            this._pocoData = pocoData;
            this._trackedObject = trackedObject;
            this.PopulateValues(trackedObject);
        }

        private void PopulateValues(T original)
        {
            T clone = original.Copy();
            foreach (PocoColumn pocoColumn in this._pocoData.Columns.Values)
            {
                this._originalValues[pocoColumn] = pocoColumn.GetColumnValue(this._pocoData, clone);
            }
        }

        public void OverrideTrackedObject(T obj)
        {
            this._trackedObject = obj;
        }

        public List<string> UpdatedColumns()
        {
            return this.Changes().Select(x => x.ColumnName).ToList();
        }

        public class Change
        {
            public string Name { get; set; }
            public string ColumnName { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
        }

        public List<Change> Changes()
        {
            List<Change> list = new List<Change>();
            foreach (KeyValuePair<PocoColumn, object> pocoColumn in this._originalValues)
            {
                object newValue = pocoColumn.Key.GetColumnValue(this._pocoData, this._trackedObject);
                if (!AreEqual(pocoColumn.Value, newValue))
                {
                    list.Add(new Change()
                    {
                        Name = pocoColumn.Key.MemberInfoData.Name,
                        ColumnName = pocoColumn.Key.ColumnName,
                        NewValue = newValue,
                        OldValue = pocoColumn.Value
                    });
                }
            }
            return list;
        }

        private static bool AreEqual(object first, object second)
        {
            if (first == null && second == null) return true;
            if (first == null) return false;
            if (second == null) return false;

            Type type = first.GetType();
            if (type.IsAClass() || type.IsArray)
            {
                return DatabaseFactory.ColumnSerializer.Serialize(first) == DatabaseFactory.ColumnSerializer.Serialize(second);
            }

            return first.Equals(second);
        }
    }
}
