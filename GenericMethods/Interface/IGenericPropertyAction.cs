using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.GenericMethods.Interface
{
    public interface IGenericPropertyAction
    {
        object GenericGetValue<T>(T t, string propName);

        void GenericSetValue<T>(T t, string propName, object value);


    }
}
