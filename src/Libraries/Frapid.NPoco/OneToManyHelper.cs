using System;
using System.Collections;

namespace Frapid.NPoco
{
    public class OneToManyHelper
    {
        public static void SetListValue<T>(Func<T, IList> listFunc, PocoMember pocoMember, object prevPoco, T poco)
        {
            IList prevList = listFunc((T)prevPoco);
            IList currentList = listFunc(poco);

            if (prevList == null && currentList != null)
            {
                prevList = pocoMember.CreateList();
                pocoMember.SetValue(prevPoco, prevList);
            }

            if (prevList != null && currentList != null)
            {
                foreach (object item in currentList)
                {
                    prevList.Add(item);
                }
            }
        }

        public static void SetForeignList<T>(Func<T, IList> listFunc, PocoMember foreignMember, object prevPoco)
        {
            if (listFunc == null || foreignMember == null)
                return;

            IList currentList = listFunc((T)prevPoco);

            if (currentList == null)
                return;

            foreach (object item in currentList)
            {
                foreignMember.SetValue(item, prevPoco);
            }
        }
    }
}
