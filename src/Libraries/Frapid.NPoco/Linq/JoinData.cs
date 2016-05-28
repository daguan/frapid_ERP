using System.Collections.Generic;

namespace Frapid.NPoco.Linq
{
    public class JoinData
    {
        public string OnSql { get; set; }
        public PocoMember PocoMember { get; set; }
        public PocoMember PocoMemberJoin { get; set; }
        public List<PocoMember> PocoMembers { get; set; }
        public JoinType JoinType { get; set; }
    }
}