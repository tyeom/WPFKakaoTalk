using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Helper;

public class SerializeHelper
{
    public static T ReadDataFromXmlFile<T>(
            string fileName,
            bool useDataContractSerialize = false) where T : class
    {
        try
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string xmlData = streamReader.ReadToEnd();
                T result =
                useDataContractSerialize ?
                DataContractSerializeDeserialize<T>(xmlData) : XmlSerializerDeserialize<T>(xmlData);

                return result;
            }
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex);
            return null;
        }
    }

    public static T DataContractSerializeDeserialize<T>(string xmlData) where T : class
    {
        try
        {
            using (StringReader reader = new StringReader(xmlData))
            {
                XmlReader xmlReader = XmlReader.Create(reader);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return serializer.ReadObject(xmlReader) as T;
            }
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex);
            return null;
        }
    }

    public static T XmlSerializerDeserialize<T>(string xmlData) where T : class
    {
        try
        {
            using (StringReader stringReader = new StringReader(xmlData))
            {
                using (XmlTextReader xmlReader = new XmlTextReader(stringReader))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(xmlReader) as T;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex);
            return null;
        }
    }

    public static void SaveDataToXml<T>(
            string fileName,
            T target,
            bool useDataContractSerialize = false)
    {
        try
        {
            using (TextWriter streamWriter = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                string xmlData =
                useDataContractSerialize ?
                DataContractSerializerSerialize(target) : XmlSerializerSerialize(target);

                streamWriter.Write(xmlData);
            }
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex);
        }
    }

    public static string DataContractSerializerSerialize<T>(T obj)
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                // settings.Encoding = Encoding.UTF8;
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(sb, settings))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(xmlWriter, obj);
                xmlWriter.Flush();

                return sb.ToString();
            }
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex);
            return string.Empty;
        }
    }

    public static string XmlSerializerSerialize<T>(T obj)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string xmlData;
            using (MemoryStream memStream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = new string(' ', 4),
                    NewLineOnAttributes = false,
                    Encoding = Encoding.UTF8
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(memStream, settings))
                {
                    serializer.Serialize(xmlWriter, obj);
                }

                xmlData = Encoding.UTF8.GetString(memStream.GetBuffer());
                xmlData = xmlData.Substring(xmlData.IndexOf('<'));
                xmlData = xmlData.Substring(0, xmlData.LastIndexOf('>') + 1);
            }

            return xmlData;
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex);
            return string.Empty;
        }
    }
}
