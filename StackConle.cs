using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UtilityNet
{
    /// <summary>
    /// 复制实体对象
    /// </summary>
    public class StackClone
    {

        /// <summary>
        /// 复制对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeObj"></param>
        /// <returns></returns>
        public static T Clone<T>(T serializeObj)
        {
            string base64String = SerializeObject<T>(serializeObj);
            return DeserializeObject<T>(base64String);
        }

        /// <summary>
        /// DataContract 对象复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enty"></param>
        /// <returns></returns>
        public static T CloneContract<T>(T enty)
        {
            T obj = default(T);
            try
            {
                var serializer = new DataContractSerializer(typeof(T));
                MemoryStream stream = new MemoryStream();
                serializer.WriteObject(stream, enty);
                stream.Seek(0, SeekOrigin.Begin);
                obj = (T)serializer.ReadObject(stream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }


        /// <summary>
        /// 序列化为Base64String。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeObj"></param>
        /// <returns></returns>
        private static string SerializeObject<T>(T serializeObj)
        {

            string base64String = string.Empty;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(ms, serializeObj);
                base64String = Convert.ToBase64String(ms.GetBuffer());
            }
            catch (SerializationException e)
            {
                throw e;
            }
            finally
            {
                ms.Close();
            }
            return base64String;
        }

        /// <summary>
        /// 从Base64反序列化。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64String"></param>
        /// <returns></returns>
        private static T DeserializeObject<T>(string base64String)
        {
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64String));
            BinaryFormatter formatter = new BinaryFormatter();
            T obj;
            try
            {
                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                obj = (T)formatter.Deserialize(ms);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ms.Close();
            }
            return obj;
        }

    }
}
