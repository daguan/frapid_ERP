using Frapid.NPoco.Expressions;

namespace Frapid.NPoco
{
    public static class ExpressionExtensions
    {
        public static int UpdateWhere<T>(this IDatabase database, T obj, string where, params object[] parameters)
        {
            SqlExpression<T> ev = database.DatabaseType.ExpressionVisitor<T>(database, database.PocoDataFactory.ForType(typeof(T)));
            ev.Where(where, parameters);
            string sql = ev.Context.ToUpdateStatement(obj);
            return database.Execute(sql, ev.Context.Params);
        }

        public static int DeleteWhere<T>(this IDatabase database, string where, params object[] parameters)
        {
            SqlExpression<T> ev = database.DatabaseType.ExpressionVisitor<T>(database, database.PocoDataFactory.ForType(typeof(T)));
            ev.Where(where, parameters);
            return database.Execute(ev.Context.ToDeleteStatement(), ev.Context.Params);
        }
    }
}
