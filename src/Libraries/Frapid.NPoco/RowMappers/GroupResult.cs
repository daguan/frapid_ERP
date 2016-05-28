using System.Collections.Generic;

namespace Frapid.NPoco.RowMappers
{
    public class GroupResult<TKey>
    {
        public TKey Key { get; set; }
        public string Item { get; set; }
        public int Count { get; set; }
        public IEnumerable<GroupResult<TKey>> SubItems { get; set; }
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Item, this.Count);
        }
    }
}