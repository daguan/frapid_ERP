using System;

namespace Frapid.NPoco
{
    public interface IColumnSerializer
    {
        string Serialize(object value);
        object Deserialize(string value, Type targeType);
    }
}