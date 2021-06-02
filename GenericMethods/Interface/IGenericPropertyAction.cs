using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingApplication___Console.GenericMethods.Interface
{
    public interface IGenericPropertyAction
    {
        object GenericGetValue<T>(T t, string propName);

        void GenericSetValue<T>(T t, string propName, object value);

        Task<object> GenericGetValueAsync<T>(T t, string propName);

        Task GenericSetValueAsync<T>(T t, string propName, object value);

    }
}
