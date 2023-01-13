////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2022 Eppie(https://eppie.io)
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

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tuvi.RestClient
{
    public class MessageQuery
    {
        internal string QueryString { get; set; }

        public MessageQuery(IEnumerable<KeyValuePair<string, string>> query)
        {
            QueryString = Convert(query);
        }

        public MessageQuery(object query)
        {

            var properties = from p in query?.GetType().GetProperties()
                             where p.CanRead
                             select new KeyValuePair<string, string>(p.Name, p.GetValue(query, null)?.ToString());

            // queryString will be set to "Id=1&State=26&Prefix=f&Index=oo"
            //string queryString = string.Join("&", properties.ToArray());

            QueryString = Convert(properties);
        }

        private static string Convert(IEnumerable<KeyValuePair<string, string>> query)
        {
            var paramString = query?.Select(param => HttpUtility.UrlEncode(param.Key) + "=" + HttpUtility.UrlEncode(param.Value));
            return string.Join("&", paramString ?? Enumerable.Empty<string>());
        }
    }
}
