using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace UtilityNet
{
    /// <summary>
    /// 将派生类的值反射到基类
    /// </summary>
    /// <typeparam name="TDerive">派生类</typeparam>
    public abstract class CloneDeriveToBase<TDerive> where TDerive : new ()
    {
        /// <summary>
        /// 复制值
        /// </summary>
        /// <typeparam name="TBase">基类</typeparam>
        /// <param name="objBase">基类对象值</param>
        /// <param name="IgnoreBase">忽略它是继承</param>
        /// <returns></returns>
        public static TDerive Clone<TBase>(TBase objBase, bool IgnoreBase = false)
        {
            
            var TDer = new TDerive();
            if (objBase == null) return TDer; 
            var derType = TDer.GetType();
            var baseType = objBase.GetType();
            if (derType.IsSubclassOf(baseType) && !IgnoreBase)
            {
                return TDer;
            }
            var bFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;
            foreach (var item in derType.GetProperties(bFlags))
            {
                try
                {
                    var Prope = baseType.GetProperty(item.Name, bFlags);
                    if (Prope != null && item.CanWrite)
                    {
                        var objValue = Prope.GetValue(objBase, null);
                        item.SetValue(TDer, objValue, null);
                    }
                }
                catch { }
            }
            return TDer;
        }
    }
}