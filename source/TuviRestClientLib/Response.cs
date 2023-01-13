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

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tuvi.RestClient
{
    public class Response
    {
        public MessageHeaders Headers { get; internal set; }
        internal virtual Task ContentAsync(HttpContent content, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class EmptyResponse : Response
    { }

    public class StringResponse : Response
    {
        public string Content { get; protected set; }

        internal override async Task ContentAsync(HttpContent content, CancellationToken cancellationToken)
        {
            if (content != null)
            {
                Content =  await content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }

    public class JsonResponse<TContent> : Response
        where TContent : struct
    {
        public TContent Content { get; protected set; }
        public JsonSerializerOptions Options { get; set; }

        internal override async Task ContentAsync(HttpContent content, CancellationToken cancellationToken)
        {
            try
            {
                Content = await content.ReadFromJsonAsync<TContent>(Options, cancellationToken).ConfigureAwait(false);
            }
            catch (NotSupportedException) // When content type is not valid
            {
                //Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                //Console.WriteLine("Invalid JSON.");
            }
        }
    }
}
