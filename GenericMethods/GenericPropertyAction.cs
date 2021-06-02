using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.GenericMethods.Interface;

namespace TradingApplication___Console.GenericMethods
{
    public class GenericPropertyAction:IGenericPropertyAction
    {
        private readonly ILogger<GenericPropertyAction> _log;

        public GenericPropertyAction(ILogger<GenericPropertyAction> log)
        {
            _log = log;
        }

        // {get,set} for generic objects
        public object GenericGetValue<T>(T t, string propName)
        {
            try
            {
                var genericType = t.GetType().GetProperty(propName);
                object obj = genericType.GetValue(t, null);
                return obj;
            }
            catch (Exception e)
            {
                _log.LogInformation("GenericGetValue error: {error}",e.Message);
                return default(object);
            }
        }
        public void GenericSetValue<T>(T t, string propName, object value)
        {
            try
            {
                var genericType = t.GetType().GetProperty(propName);
                genericType.SetValue(t, Convert.ChangeType(value, genericType.PropertyType), null);
            }
            catch (Exception e)
            {
                _log.LogInformation("GenericSetValue error: {error}",e.Message);
            }

        }

        public Task<object> GenericGetValueAsync<T>(T t, string propName)
        {
            return Task.Run(() => GenericGetValue(t, propName));
        }

        public Task GenericSetValueAsync<T>(T t, string propName, object value)
        {
            return Task.Run(() => GenericSetValue(t, propName, value));
        }



    }
}
