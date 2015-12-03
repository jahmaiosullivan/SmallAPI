using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmallApi.WorkerRole
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var js = JsonSerializer.Create(settings);
            var jw = new StringWriter();
            js.Serialize(jw, obj);
            return jw.ToString();
        }

        public static string RemoveChars(this string input, char[] charsToRemove)
        {
            foreach (var c in charsToRemove)
            {
                input = input.Replace(c.ToString(), "");
            }
            return input;
        }

        public static long GetJavascriptTimestamp(this System.DateTime input)
        {
            var span = new System.TimeSpan(System.DateTime.Parse("1/1/1970").Ticks);
            var time = input.Subtract(span);
            return time.Ticks / 10000;
        }

        public static byte[] ToByteArray(this object source)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }

        public static T ToObject<T>(this byte[] source)
        {
            var memoryStream = new MemoryStream(source);
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Position = 0;
            return (T)binaryFormatter.Deserialize(memoryStream);

        }
    }
}