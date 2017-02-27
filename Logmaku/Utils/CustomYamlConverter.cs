using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Logmaku.Utils
{
    public class CustomYamlConverter : IYamlTypeConverter
    {
        private static readonly Dictionary<Type, Func<string, object>> Converters = new Dictionary
            <Type, Func<string, object>>
            {
                {typeof(FontFamily), _ => new FontFamily(_)},
                {typeof(Color), ColorConverter.ConvertFromString},
                {typeof(Duration), _ => new Duration(TimeSpan.Parse(_, CultureInfo.DefaultThreadCurrentUICulture))},
                {typeof(Encoding), Encoding.GetEncoding}
            };

        public bool Accepts(Type type) => Converters.ContainsKey(type);

        public object ReadYaml(IParser parser, Type type)
        {
            var result = Converters[type](((Scalar) parser.Current).Value);
            parser.MoveNext();
            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}