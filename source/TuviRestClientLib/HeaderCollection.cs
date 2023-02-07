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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Tuvi.RestClient.Utils;

[assembly: InternalsVisibleTo("Tuvi.RestClient.Test")]
namespace Tuvi.RestClient
{
    public class HeaderCollection : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        private readonly bool _headerValidation;
        private IEnumerable<KeyValuePair<string, IEnumerable<string>>> _headers;
        private IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers
        {
            get => _headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>();
            set => _headers = value;
        }

        public HeaderCollection(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers, bool headerValidation = true)
        {
            if (headers is null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            _headerValidation = headerValidation;
            Headers = headers;
        }

        public HeaderCollection(IEnumerable<KeyValuePair<string, string>> headers, bool headerValidation = false)
        {
            if (headers is null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            _headerValidation = headerValidation;
            Headers = headers.Select((header) =>
            {
                return new KeyValuePair<string, IEnumerable<string>>(header.Key, new[] { header.Value });
            });
        }

        public HeaderCollection(IEnumerable<(string, string)> headers, bool headerValidation = false)
        {
            if (headers is null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            _headerValidation = headerValidation;
            Headers = headers.Select((header) =>
            {
                return new KeyValuePair<string, IEnumerable<string>>(header.Item1, new[] { header.Item2 });
            });
        }

        public void Add(IEnumerable<(string, string)> headers)
        {
            if (headers is null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Add(headers.Select((header) =>
            {
                return new KeyValuePair<string, IEnumerable<string>>(header.Item1, new[] { header.Item2 });
            }));
        }

        public void Add(IEnumerable<KeyValuePair<string, string>> headers)
        {
            if (headers is null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Add(headers.Select((header) =>
            {
                return new KeyValuePair<string, IEnumerable<string>>(header.Key, new[] { header.Value });
            }));
        }

        public void Add(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            if (headers is null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Headers = Headers.Concat(headers);
        }

        internal void UpdateHeaders(HttpRequestMessage httpRequest)
        {
            foreach (var header in Headers)
            {
                if (header.Key is null || header.Value is null)
                {
                    continue;
                }

                if (_headerValidation)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
                else
                {
                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return Headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static HeaderCollection Create(object obj, bool headerValidation = false)
        {
            return new HeaderCollection(ObjectConverter.Convert(obj), headerValidation);
        }
    }
}
