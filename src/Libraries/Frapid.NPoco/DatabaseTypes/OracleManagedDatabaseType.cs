namespace Frapid.NPoco.DatabaseTypes
{
    public class OracleManagedDatabaseType : OracleDatabaseType
    {
        public override string GetProviderName()
        {
            return "Oracle.ManagedDataAccess.Client";
        }
    }
}
