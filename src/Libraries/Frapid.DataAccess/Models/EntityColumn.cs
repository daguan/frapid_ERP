using Frapid.NPoco.FluentMappings;

namespace Frapid.DataAccess.Models
{
    public class EntityColumn : IPoco
    {
        private string _columnName;
        private string _value;

        public string ColumnName
        {
            get { return this._columnName; }
            set
            {
                this._columnName = value;
                this.PropertyName = Inflector.ToTitleCase(value).Replace("_", "").Replace(" ", "");
            }
        }

        public bool IsNullable { get; set; }
        public string DbDataType { get; set; }

        public string Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                if (value.StartsWith("nextval"))
                {
                    this.IsSerial = true;
                    this._value = string.Empty;
                }
            }
        }

        public int MaxLength { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsSerial { get; set; }
        public string PropertyName { get; set; }
        public string DataType { get; set; }
    }
}