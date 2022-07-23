using System;
using System.IO;
using System.Collections.Generic;

namespace ReadIni
{
    class Program
    {
        static void Main(string[] args)
        {
            var iniFileClass = ToObj<IniFile>(Console.ReadLine());            
        }

        public static T ToObj<T>(string filename) where T : class, new()
        {
            var file = File.ReadAllText(filename);
            var parsedText = Parser(file);
            
            Type dynamicClassType = typeof(T);
            var dynamicClassConstructor = dynamicClassType.GetConstructor(new Type[] { });
            var dynamicObject = dynamicClassConstructor.Invoke(new object[] { });

            foreach(var property in dynamicClassType.GetProperties())
            {
                if (parsedText.ContainsKey(property.Name))
                {
                    var value = Convert.ChangeType(parsedText[property.Name], property.PropertyType);
                    property.SetValue(dynamicObject, value);
                }
            }
            return dynamicObject as T;
        }

        private static Dictionary<string, object> Parser(string file)
        {
            Dictionary<string, object> keyPairValue = new Dictionary<string, object>();
            var parsedText = file.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var text in parsedText)
            {
                var keyPair = text.Split('=');
                keyPairValue.Add(keyPair[0].Trim(), keyPair[1].Trim());
            }
            return keyPairValue;
        }
    }

   
}
