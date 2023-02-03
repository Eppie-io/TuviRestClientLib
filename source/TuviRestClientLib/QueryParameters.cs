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
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Tuvi.RestClient
{
    public class QueryParameters
    {
        private NameValueCollection Collection { get; set; }

        public override string ToString()
        {
            return Collection.ToString();
        }

        public QueryParameters(string query)
        {
            Collection = HttpUtility.ParseQueryString(query, System.Text.Encoding.Default);
        }

        public QueryParameters(IEnumerable<KeyValuePair<string, string>> query)
        {
            Collection = Create(query);
        }

        public QueryParameters(IEnumerable<(string key, string value)> query)
        {
            Collection = Create(query.Select(item => new KeyValuePair<string, string>(item.key, item.value)));
        }

        public static QueryParameters Create(object obj)
        {
            return new QueryParameters(Utils.ObjectConverter.Convert(obj));
        }

        private static NameValueCollection Create(IEnumerable<KeyValuePair<string, string>> query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var collection = HttpUtility.ParseQueryString(string.Empty);

            foreach (var parameter in query)
            {
                if(IsKeyCorrect(parameter.Key))
                {
                    collection.Add(parameter.Key, parameter.Value);
                }
            }

            return collection;
        }

        private static bool IsKeyCorrect(string key)
        {
            return !string.IsNullOrWhiteSpace(key);
        }
    }
}
