using System;

namespace Frapid.DataAccess.Models
{
    public sealed class Filter:IPoco
    {
        public long FilterId { get; set; }
        public string ObjectName { get; set; }
        public string FilterName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public string FilterStatement { get; set; }
        public string ColumnName { get; set; }
        public string PropertyName { get; set; }
        public int FilterCondition { get; set; }
        public object FilterValue { get; set; }
        public object FilterAndValue { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}