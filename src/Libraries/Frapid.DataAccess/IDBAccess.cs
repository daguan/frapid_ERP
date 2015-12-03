namespace Frapid.DataAccess
{
    public interface IDbAccess
    {
        bool HasAccess { get; }
        void Validate(AccessTypeEnum type, long loginId, string catalog, bool noException);
    }
}