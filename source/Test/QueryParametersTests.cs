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

namespace Tuvi.RestClient.Test
{
    public class QueryParametersTests
    {
        [Test]
        [TestCase("")]
        [TestCase("param1=value1&param2=value2")]
        [TestCase("param2=value2&param1=value1")]
        [TestCase("param1=&param2=")]
        [TestCase("param1=foo bar", "param1=foo+bar")]
        [TestCase("param1=!_.0-9A-Za-z(*)")]
        [TestCase("""param1=@#$%^=":;<>,?/\|""", "param1=%40%23%24%25%5e%3d%22%3a%3b%3c%3e%2c%3f%2f%5c%7c")]
        [TestCase("param1=%2b&param2=%26&param3=%&param4= &param5=%20", "param1=%2b&param2=%26&param3=%25&param4=+&param5=+")]
        [TestCase("param1=%40%23%24%25%5e%3d%22%3a%3b%3c%3e%2c%3f%2f%5c%7c")]
        public void StringTest(string query, string? result = null)
        {
            result ??= query;

            var queryParams = new QueryParameters(query);
            Assert.That(queryParams.ToString(), Is.EqualTo(result));
        }

        [Test]
        [TestCaseSource(typeof(Data.QueryParametersData), nameof(Data.QueryParametersData.QueryTupleParams))]
        [TestCaseSource(typeof(Data.QueryParametersData), nameof(Data.QueryParametersData.QueryUnicodeParams))]
        public void TupleTest(IEnumerable<(string, string)> query, string result)
        {
            var queryParams = new QueryParameters(query);
            Assert.That(queryParams.ToString(), Is.EqualTo(result));
        }

        [Test]
        [TestCaseSource(typeof(Data.QueryParametersData), nameof(Data.QueryParametersData.QueryPairParams))]
        public void PairTest(IEnumerable<KeyValuePair<string, string?>> query, string result)
        {
            var queryParams = new QueryParameters(query);
            Assert.That(queryParams.ToString(), Is.EqualTo(result));
        }

        [Test]
        [TestCaseSource(typeof(Data.QueryParametersData), nameof(Data.QueryParametersData.QueryStructParams))]
        public void StructTest(object query, string result)
        {
            var queryParams = QueryParameters.Create(query);
            Assert.That(queryParams.ToString(), Is.EqualTo(result));
        }
    }
}
