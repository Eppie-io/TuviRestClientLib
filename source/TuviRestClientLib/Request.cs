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

using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tuvi.RestClient
{
    public class Request
    {
        public HeaderCollection Headers { get; set; }

        internal virtual Task<HttpContent> GetContentAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpContent>(null);
        }
    }

    public class EmptyRequest : Request
    { }

    public class StringRequest : Request
    {
        public string Payload { get; set; }

        internal override Task<HttpContent> GetContentAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpContent>(new StringContent(Payload));
        }
    }

    public class JsonRequest<TJsonPayload> : Request
    {
        public TJsonPayload Payload { get; set; }
        public JsonSerializerOptions Options { get; set; }

        internal override Task<HttpContent> GetContentAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpContent>(JsonContent.Create(inputValue: Payload, options: Options));
        }
    }
}
