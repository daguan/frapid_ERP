using System;
using System.Collections.Generic;
using System.Threading;

namespace Frapid.NPoco
{
    public class EnumMapper : IDisposable
    {
        readonly Dictionary<Type, Dictionary<string, object>> _stringsToEnums = new Dictionary<Type, Dictionary<string, object>>();
        readonly Dictionary<Type, Dictionary<int, string>> _enumNumbersToStrings = new Dictionary<Type, Dictionary<int, string>>();
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        
        public object EnumFromString(Type type, string value)
        {
            this.PopulateIfNotPresent(type);
            if (!this._stringsToEnums[type].ContainsKey(value))
            {
                throw new Exception(string.Format("The value '{0}' could not be found for Enum '{1}'", value, type));
            }
            return this._stringsToEnums[type][value];
        }

        public string StringFromEnum(object theEnum)
        {
            Type typeOfEnum = theEnum.GetType();
            this.PopulateIfNotPresent(typeOfEnum);
            return this._enumNumbersToStrings[typeOfEnum][(int)theEnum];
        }

        void PopulateIfNotPresent(Type type)
        {
            this._lock.EnterUpgradeableReadLock();
            try
            {
                if (!this._stringsToEnums.ContainsKey(type))
                {
                    this._lock.EnterWriteLock();
                    try
                    {
                        this.Populate(type);
                    }
                    finally
                    {
                        this._lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                this._lock.ExitUpgradeableReadLock();
            }
        }

        void Populate(Type type)
        {
            Array values = Enum.GetValues(type);
            this._stringsToEnums[type] = new Dictionary<string, object>(values.Length);
            this._enumNumbersToStrings[type] = new Dictionary<int, string>(values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                object value = values.GetValue(i);
                this._stringsToEnums[type].Add(value.ToString(), value);
                this._enumNumbersToStrings[type].Add((int)value, value.ToString());
            }
        }

        public void Dispose()
        {
            this._lock.Dispose();
        }
    }
}
