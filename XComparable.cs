using System;
using System.Reflection;

namespace UtilityNet
{
    public sealed class XComparer<T> 
    {
        /// <summary>
        /// 将一个实体实数据与另一个副本进行比较。
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <returns></returns>
        public static bool Compare(T objA, T objB)
        {
            if (objA != null && objB == null) return false;
            if (objA == null && objB != null) return false;
            foreach (PropertyInfo key in objA.GetType().GetProperties())
            {
                object AValue = key.GetValue(objA, null);
                object BValue = objB.GetType().GetProperty(key.Name).GetValue(objB, null);
                if (!object.Equals(AValue, BValue)) return false;
            }
            return true;
        }
    }
}
