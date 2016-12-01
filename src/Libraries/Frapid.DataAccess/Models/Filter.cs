using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.Mapper.Decorators;

namespace Frapid.DataAccess.Models
{
    [TableName("config.filters")]
    [PrimaryKey("filter_id", AutoIncrement = true)]
    public sealed class Filter : IPoco
    {
        private static readonly Dictionary<Type, string> Aliases = new Dictionary<Type, string>
        {
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(object), "object"},
            {typeof(bool), "bool"},
            {typeof(char), "char"},
            {typeof(string), "string"},
            {typeof(void), "void"}
        };

        private string _dataType;
        public long FilterId { get; set; }
        public string ObjectName { get; set; }
        public string FilterName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public string FilterStatement { get; set; }
        public string ColumnName { get; set; }

        [Ignore]
        public string PropertyName { get; set; }

        public int FilterCondition { get; set; }
        public string FilterValue { get; set; }
        public string FilterAndValue { get; set; }

        #region Type

        public string DataType
        {
            get { return this._dataType; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                this._dataType = value;

                try
                {
                    var type = Aliases.FirstOrDefault(x => x.Value.Equals(value)).Key;
                    this.Type = type ?? Type.GetType(value);
                }
                catch
                {
                    //Swallow                
                }
            }
        }

        [Ignore]
        public Type Type { get; set; }

        #endregion
    }
}