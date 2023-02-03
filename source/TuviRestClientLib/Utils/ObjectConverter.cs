////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2023 Eppie(https://eppie.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tuvi.RestClient.Utils
{
    internal static class ObjectConverter
    {
        public static IEnumerable<KeyValuePair<string, string>> Convert(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            string GetName(PropertyInfo property)
            {
                var attribute = property.GetCustomAttribute<ParameterNameAttribute>();

                if (attribute is null)
                {
                    return property.Name;
                }

                return attribute.Name;
            }

            string GetValue(PropertyInfo property)
            {
                var delegateConverter = property.GetCustomAttribute<ParameterConverterAttribute>();
                if (delegateConverter != null)
                {
                    return delegateConverter.Converter(property.GetValue(obj));
                }

                return property.GetValue(obj)?.ToString();
            }

            return from property in obj.GetType().GetProperties()
                   where property.CanRead
                   select new KeyValuePair<string, string>(GetName(property), GetValue(property));
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ParameterNameAttribute : Attribute
    {
        public ParameterNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ParameterConverterAttribute : Attribute
    {
        public delegate string Convert(object obj);

        public ParameterConverterAttribute(Type delegateType, string delegateName)
        {
            DelegateType = delegateType;
            DelegateName = delegateName;
            Converter = Delegate.CreateDelegate(typeof(Convert), DelegateType, DelegateName) as Convert;
        }

        public Convert Converter { get; }
        public Type DelegateType { get; }
        public string DelegateName { get; }
    }
}
