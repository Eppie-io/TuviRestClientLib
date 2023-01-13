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

using NUnit.Framework;
using System.Collections;
using Tuvi.RestClient.Utils;

namespace Tuvi.RestClient.Test.Data
{
    internal partial class QueryParametersData
    {
        public struct StructData
        {
            public string StringParam { get; set; }
            public int IntParam { get; set; }
            public double DoubleParam { get; set; }
            public bool? BoolParam { get; set; }
        }

        public struct AttributeData
        {
            [ParameterName("WellNamedParam")]
            public int? WrongNamedParam { get; set; }

            [ParameterConverter(typeof(AttributeData), nameof(Convert))]
            public int? ConvertParam { get; set; }

            public static string? Convert(object? obj)
            {
                return obj is null ? null : $"Value-is({obj})";
            }
        }

        public static IEnumerable QueryTupleParams
        {
            get
            {
                yield return new TestCaseData(new[] { ("param1", "value1"), ("param2", "value2") }, "param1=value1&param2=value2");
                yield return new TestCaseData(new[] { ("param2", "value2"), ("param1", "value1") }, "param2=value2&param1=value1");
                yield return new TestCaseData(new (string, string?)[] { ("param", "value"), ("param2", null) }, "param=value");
                yield return new TestCaseData(new[] { ("param", "foo bar") }, "param=foo+bar");
                yield return new TestCaseData(new[] { ("param", "!_.0-9A-Za-z(*)") }, "param=!_.0-9A-Za-z(*)");
                yield return new TestCaseData(new[] { ("param", """'@#$%^=":;<>,?/\|+&""") }, "param=%27%40%23%24%25%5e%3d%22%3a%3b%3c%3e%2c%3f%2f%5c%7c%2b%26");
            }
        }

        public static IEnumerable QueryPairParams
        {
            get
            {
                yield return new TestCaseData(new Dictionary<string, string> { { "param1", "value1" }, { "param2", "value2" } }, "param1=value1&param2=value2");
                yield return new TestCaseData(new Dictionary<string, string> { { "param2", "value2" }, { "param1", "value1" } }, "param2=value2&param1=value1");
                yield return new TestCaseData(new Dictionary<string, string?> { { "param", "value" }, { "param2", null } }, "param=value");
                yield return new TestCaseData(new Dictionary<string, string> { { "param", "foo bar" } }, "param=foo+bar");
                yield return new TestCaseData(new Dictionary<string, string> { { "param", "!_.0-9A-Za-z(*)" } }, "param=!_.0-9A-Za-z(*)");
                yield return new TestCaseData(new Dictionary<string, string> { { "param", """'@#$%^=":;<>,?/\|+&""" } }, "param=%27%40%23%24%25%5e%3d%22%3a%3b%3c%3e%2c%3f%2f%5c%7c%2b%26");
            }
        }

        public static IEnumerable QueryStructParams
        {
            get
            {
                yield return new TestCaseData(new StructData
                {
                    StringParam = "!_.0-9A-Za-z(*)",
                    IntParam = int.MinValue,
                    DoubleParam = double.E,
                }, $"{nameof(StructData.StringParam)}=!_.0-9A-Za-z(*)&{nameof(StructData.IntParam)}={int.MinValue}&{nameof(StructData.DoubleParam)}={double.E}");

                yield return new TestCaseData(new StructData
                {
                    StringParam = """' @#$%^=":;<>,?/\|+&""",
                    IntParam = int.MaxValue,
                    DoubleParam = double.Pi,
                    BoolParam = false,
                }, $"{nameof(StructData.StringParam)}=%27+%40%23%24%25%5e%3d%22%3a%3b%3c%3e%2c%3f%2f%5c%7c%2b%26&{nameof(StructData.IntParam)}={int.MaxValue}&{nameof(StructData.DoubleParam)}={double.Pi}&{nameof(StructData.BoolParam)}={bool.FalseString}");

                yield return new TestCaseData(new AttributeData
                {
                    WrongNamedParam = 0,
                    ConvertParam = int.MaxValue,
                }, $"WellNamedParam=0&{nameof(AttributeData.ConvertParam)}={AttributeData.Convert(int.MaxValue)}");
            }
        }
    }
}
