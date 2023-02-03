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
    public class HeaderCollectionTest
    {
        [TestCaseSource(typeof(Data.HeaderCollectionTestData), nameof(Data.HeaderCollectionTestData.HeaderTupleParams))]
        public void TupleTest(IEnumerable<(string?, string?)> headers, IEnumerable<(string?, string?)>? result)
        {
            result ??= headers;

            var headerCollection = new HeaderCollection(headers);

            using var request = new HttpRequestMessage();
            headerCollection.UpdateHeaders(request);

            foreach ((var header, var value) in result ?? Enumerable.Empty<(string?, string?)>())
            {
                if (header is not null)
                {
                    var values = request.Headers.GetValues(header);
                    Assert.That(values, Does.Contain(value));
                }
            }
        }

        [TestCaseSource(typeof(Data.HeaderCollectionTestData), nameof(Data.HeaderCollectionTestData.HeaderPairParams))]
        public void PairTest(
            IEnumerable<KeyValuePair<string, IEnumerable<string?>?>> headers,
            IEnumerable<(string, string)>? addedHeaders = null,
            IEnumerable<(string, bool)>? notHeaders = null)
        {
            var headerCollection = new HeaderCollection(headers, false);

            using var request = new HttpRequestMessage();
            headerCollection.UpdateHeaders(request);

            foreach ((var header, var value) in addedHeaders ?? Enumerable.Empty<(string, string)>())
            {
                var values = request.Headers.GetValues(header);
                Assert.That(values, Does.Contain(value));
            }

            foreach ((var header, var assert) in notHeaders ?? Enumerable.Empty<(string, bool)>())
            {
                if (assert)
                {
                    Assert.Throws<FormatException>(() => request.Headers.Contains(header));
                }
                else
                {
                    Assert.That(request.Headers.Contains(header), Is.False);
                }
            }
        }

        [Test]
        public void NullTest()
        {
            {
                IEnumerable<KeyValuePair<string, IEnumerable<string>>>? headers = null;
                var headerCollection = new HeaderCollection(headers, false);

                using var request = new HttpRequestMessage();
                headerCollection.UpdateHeaders(request);

                Assert.That(request.Headers, Is.Empty);
            }

            {
                IEnumerable<(string, string)>? headers = null;
                var headerCollection = new HeaderCollection(headers, false);

                using var request = new HttpRequestMessage();
                headerCollection.UpdateHeaders(request);

                Assert.That(request.Headers, Is.Empty);
            }
        }
    }
}
