using Frapid.NPoco.Expressions;

namespace Frapid.NPoco.Linq
{
    public interface ISimpleQueryProviderExpression<TModel>
    {
        SqlExpression<TModel> AtlasSqlExpression { get; }
    }
}