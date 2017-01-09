using System;

namespace UtilityNet
{
    /// <summary>
    /// 泛型转换
    /// </summary>
    public class ITryParse
    {
        /// <summary>
        /// 泛型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T TryParse<T>(string value)
        {
            return TryParse<T>(value, default(T));
        }

        public static T TryParse<T>(string value, T defaultValue)
        {
            object result = null;
            try
            {
                if (TryParse(typeof(T), value, out result))
                {
                    return (T)result;
                }
            }
            catch { }
            return defaultValue;
        }
        public static bool TryParse<T>(string value, out T result)
        {
            object obj = null;
            if (TryParse(typeof(T), value, out obj))
            {
                result = (T)obj;
                return true;
            }
            result = default(T);
            return false;

        }

        public static bool TryParse(Type type, string value, out object result)
        {
            result = null;
            if (value == null)
            {
                return false;
            }

            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                type = type.GetGenericArguments()[0];
            }

            object obj = null;
            bool flag = false;
            if (type.IsEnum)
            {
                try
                {
                    obj = Enum.Parse(type, value, true);
                    flag = true;
                }catch { }
            }
            else if (type == typeof(Guid))
            {
                try
                {
                    obj = new Guid(value);
                    flag = true;
                }
                catch { }
            }
            else 
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean :
                        bool mybool;
                        flag = bool.TryParse(value, out mybool);
                        obj = mybool;
                        break;

                    case TypeCode.Byte :
                        byte mybyte;
                        flag = byte.TryParse(value, out mybyte);
                        obj = mybyte;
                        break;

                    case TypeCode.Char :
                        char mychar;
                        flag = char.TryParse(value, out mychar);
                        obj = mychar;
                        break;

                    case TypeCode.DateTime :
                        DateTime mydatetime;
                        flag = DateTime.TryParse(value, out mydatetime);
                        obj = mydatetime;
                        break;

                    case TypeCode.Decimal :
                        decimal mydecimal;
                        flag = decimal.TryParse(value, out mydecimal);
                        obj = mydecimal;
                        break;

                    case TypeCode.Double :
                        double mydouble;
                        flag = double.TryParse(value, out mydouble);
                        obj = mydouble;
                        break;

                    case TypeCode.Int16 :
                        short myshort;
                        flag = short.TryParse(value, out myshort);
                        obj = myshort;
                        break;
                    case TypeCode.Int32 :
                        int myint;
                        flag = int.TryParse(value, out myint);
                        obj = myint;
                        break;

                    case TypeCode.Int64 :
                        long mylong;
                        flag = long.TryParse(value, out mylong);
                        obj = mylong;
                        break;

                    case TypeCode.SByte :
                        sbyte mysbyte;
                        flag = sbyte.TryParse(value, out mysbyte);
                        obj = mysbyte;
                        break;

                    case TypeCode.Single:
                        float myfloat;
                        flag = float.TryParse(value, out myfloat);
                        obj = myfloat;
                        break;

                    case TypeCode.UInt16:
                        ushort myushort;
                        flag = ushort.TryParse(value, out myushort);
                        obj = myushort;
                        break;

                    case TypeCode.UInt32:
                        uint myuint;
                        flag = uint.TryParse(value, out myuint);
                        obj = myuint;
                        break;

                    case TypeCode.UInt64:
                        ulong myulong;
                        flag = ulong.TryParse(value, out myulong);
                        obj = myulong;
                        break;
       
                    default :
                        obj = value;
                        flag = true;
                        break;
                }
            }
            result = obj;
            return flag;

        }
        
    }
}
