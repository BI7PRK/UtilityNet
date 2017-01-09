using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace UtilityNet
{
    public class SerializerHelper
    {
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string file)
        {
            var result = default(T);
            try
            {
                if (File.Exists(file))
                {
                    FileStream stream = new FileStream(file, FileMode.Open);
                    var xs = new XmlSerializer(typeof(T));
                    result = (T)xs.Deserialize(stream);
                    xs = null;
                    stream.Close();
                }
            }
            catch (Exception)
            {

                return default(T);
            }
           
            return result;
        }
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public static bool XmlSerialize<T>(string file, T objArgs)
        {
            try
            {
                var info = new FileInfo(file);
                if (!Directory.Exists(info.DirectoryName))
                {
                    Directory.CreateDirectory(info.DirectoryName);
                }

                var xs = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8);
                xs.Serialize(sw, objArgs);
                sw.Close();
                return true;
            }
            catch (Exception)
            {

            }

            return false;
        }
        /// <summary>
        /// BinaryFormatter 反序列化
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(string file)
        {
            object instance = default(T);
            try
            {
                if (new FileInfo(file).Exists)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(file, FileMode.Open);
                    instance = formatter.Deserialize(stream);
                    stream.Close();
                    formatter = null;
                }
            }
            catch
            {
            }
            return (T)instance;
        }
        /// <summary>
        /// BinaryFormatter 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        public static bool BinarySerializer<T>(string file, T stack)
        {
            if (stack is T)
            {
                try
                {
                    var info = new FileInfo(file);
                    if (!Directory.Exists(info.DirectoryName))
                    {
                        Directory.CreateDirectory(info.DirectoryName);
                    }

                    BinaryFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    formatter.Serialize(stream, stack);
                    stream.Close();
                    formatter = null;
                    return true;
                }
                catch { }
            }
            return false;
        }


        /// <summary>
        /// 序列化为Base64String。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeObj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T serializeObj)
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
        public static T DeserializeObject<T>(string base64String)
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
