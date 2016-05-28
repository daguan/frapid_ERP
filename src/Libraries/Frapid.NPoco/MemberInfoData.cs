using System;
using System.Reflection;

namespace Frapid.NPoco
{
    public class MemberInfoData : IEquatable<MemberInfoData>
    {
        public MemberInfo MemberInfo
        {
            get;
            private set;
        }

        public Type DeclaringType
        {
            get;
            private set;
        }

        public Type MemberType
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public MemberInfoData(string name, Type memberType, Type declaringType)
        {
            this.Name = name;
            this.MemberType = memberType;
            this.DeclaringType = declaringType;
        }

        public MemberInfoData(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
            this.Name = memberInfo.Name;
            this.MemberType = memberInfo.GetMemberInfoType();
            this.DeclaringType = memberInfo.DeclaringType;
        }

        public bool Equals(MemberInfoData other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(this.Name, other.Name) && Equals(this.MemberType, other.MemberType) && Equals(this.DeclaringType, other.DeclaringType);
        }

        public static bool operator ==(MemberInfoData left, MemberInfoData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MemberInfoData left, MemberInfoData right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((MemberInfoData)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.MemberType != null ? this.MemberType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.DeclaringType != null ? this.DeclaringType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}