using System;
using System.Collections.Generic;
using System.Text;
using TradingApplication___Console.GenericMethods.Interface;

namespace TradingApplication___Console.GenericMethods
{
    public class GenericPropertyAction:IGenericPropertyAction
    {

        // {get,set} for generic objects
        public object GenericGetValue<T>(T t, string propName)
        {
            var genericType = t.GetType().GetProperty(propName);
            object obj = genericType.GetValue(t, null);
            return obj;
        }
        public void GenericSetValue<T>(T t, string propName, object value)
        {
            var genericType = t.GetType().GetProperty(propName);
            genericType.SetValue(t, Convert.ChangeType(value, genericType.PropertyType), null);

        }


    }
}
