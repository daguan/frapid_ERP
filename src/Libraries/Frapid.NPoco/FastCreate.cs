using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Frapid.NPoco
{
    public class FastCreate
    {
        private readonly Type _type;
        private readonly MapperCollection _mapperCollection;

        public FastCreate(Type type, MapperCollection mapperCollection)
        {
            this._type = type;
            this._mapperCollection = mapperCollection;
            this.CreateDelegate = this.GetCreateDelegate();
        }

        public Func<DbDataReader, object> CreateDelegate { get; set; }

        public object Create(DbDataReader dataReader)
        {
            try
            {
                return this.CreateDelegate(dataReader);
            }
            catch (Exception exception)
            {
                throw new Exception("Error trying to create type " + this._type, exception);
            }
        }

        private Func<DbDataReader, object> GetCreateDelegate()
        {
            if (this._mapperCollection.HasFactory(this._type))
                return dataReader => this._mapperCollection.GetFactory(this._type)(dataReader);

            ConstructorInfo constructorInfo = this._type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault(x => x.GetParameters().Length == 0);
            if (constructorInfo == null)
                return _ => null;

            // var poco=new T()
            DynamicMethod constructor = new DynamicMethod(Guid.NewGuid().ToString(), this._type, new[] { typeof(DbDataReader) }, true);
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Ret);

            try
            {
                Delegate del = constructor.CreateDelegate(Expression.GetFuncType(typeof(DbDataReader), typeof(object)));
                return del as Func<DbDataReader, object>;
            }
            catch (Exception exception)
            {
                throw new Exception("Error trying to create type " + this._type, exception);
            }
        }
    }
}